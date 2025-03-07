namespace SurveyBasket.Api.Abstractions.Consts;

public static class DefaultRoles
{

    public partial class Admin
    {
        public const string Name = nameof(Admin);
        public const string Id = "7507f5d6-d69d-4ce4-955e-49e5611f46dc";
        public const string ConcurrencyStamp = "c230c815-ddbd-420e-adf7-f270056c2d59";
    }




    public partial class Member
    {
        public const string Name = nameof(Member);
        public const string Id = "4097f06e-a127-4960-8a9b-e4cbba84bf4f";
        public const string ConcurrencyStamp = "0439e59f-86fb-4706-b2b4-de1e795fda32";
    }
}
