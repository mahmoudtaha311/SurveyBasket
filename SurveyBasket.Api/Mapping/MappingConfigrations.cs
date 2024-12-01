namespace SurveyBasket.Api.Mapping;

public class MappingConfigrations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<Poll, PollResponse>()
        //    .Map(dest => dest.Notice, sorc => sorc.Description);
    }
}
