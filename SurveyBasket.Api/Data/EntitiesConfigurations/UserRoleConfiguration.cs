namespace SurveyBasket.Api.Data.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {

        //default data


        builder.HasData(new IdentityUserRole<string>
        {
            UserId = DefaultUser.Admin.Id,
            RoleId = DefaultRoles.Admin.Id,
        });

    }
}
