namespace SurveyBasket.Api.Data.EntitiesConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {


        //default data
        var permisiions = Permissions.GetAllPermissions();
        var adminClaims = new List<IdentityRoleClaim<string>>();

        for (var i = 0; i < permisiions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = permisiions[i],
                RoleId = DefaultRoles.Admin.Id
            });


        }
        builder.HasData(adminClaims);

    }
}
