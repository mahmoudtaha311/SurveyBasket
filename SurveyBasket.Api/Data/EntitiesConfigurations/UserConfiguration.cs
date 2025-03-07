namespace SurveyBasket.Api.Data.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("Userid");
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);


        //default data


        builder.HasData(new ApplicationUser
        {
            Id = DefaultUser.Admin.Id,
            FirstName = "Survey Basket",
            LastName = "Admin",
            UserName = DefaultUser.Admin.Email,
            NormalizedUserName = DefaultUser.Admin.Email.ToUpper(),
            Email = DefaultUser.Admin.Email,
            NormalizedEmail = DefaultUser.Admin.Email.ToUpper(),
            EmailConfirmed = true,
            ConcurrencyStamp = DefaultUser.Admin.ConcurrencyStamp,
            SecurityStamp = DefaultUser.Admin.SecurityStamp,
            PasswordHash = DefaultUser.Admin.Password,

        });

    }
}
