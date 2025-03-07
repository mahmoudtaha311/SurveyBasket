
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider(IOptions<JwtOptions> option) : IJwtProvider
{
    public readonly JwtOptions _JwtOptions = option.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        Claim[] claims =

            [

            new (JwtRegisteredClaimNames.Sub,user.Id),
            new (JwtRegisteredClaimNames.Email,user.Email!),
            new (JwtRegisteredClaimNames.GivenName,user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName,user.LastName),
            new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new (nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray),
            new (nameof(permissions),JsonSerializer.Serialize(permissions),JsonClaimValueTypes.JsonArray)
            ];
        var symmetricsecutitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtOptions.key));

        var signingcredentials = new SigningCredentials(symmetricsecutitykey, SecurityAlgorithms.HmacSha256);

        var expiresIn = _JwtOptions.expiryminutes;
        var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);
        var token = new JwtSecurityToken(

            issuer: _JwtOptions.issuer,
            audience: _JwtOptions.audience,
            signingCredentials: signingcredentials,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            claims: claims
            );



        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn * 60);
    }

    public string? ValidateToken(string token)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var symmetricsecutitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtOptions.key));

        try
        {
            tokenhandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricsecutitykey,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var JwtToken = (JwtSecurityToken)validatedToken;


            return JwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

        }
        catch
        {

            return null;
        }
    }

}
