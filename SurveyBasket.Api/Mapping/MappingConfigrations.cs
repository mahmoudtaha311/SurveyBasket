namespace SurveyBasket.Api.Mapping;

public class MappingConfigrations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
       

        config.NewConfig<QuestionRequest, Question>()
            .Ignore(nameof(Question.Answers));

        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
           .Map(dest => dest, src => src.user)
           .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<CreateUserRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Email)
           .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<UpdateUserRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Email)
           .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper());
    }
}
