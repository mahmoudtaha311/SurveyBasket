
namespace SurveyBasket.Api.Authentication;

public class JwtOptions
{
    public static string SectionName = "jwt";
    [Required]
    public string key { get; set; } = string.Empty;
    [Required]
    public string issuer { get; set; } = string.Empty;
    [Required]
    public string audience { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int expiryminutes { get; set; }
}
