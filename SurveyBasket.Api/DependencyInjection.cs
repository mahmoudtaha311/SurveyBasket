using FluentValidation.AspNetCore;
using MapsterMapper;
using System.Reflection;

namespace SurveyBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddControllers();

        //add swagger
        services.AddSwaggerServices();

        services.AddScoped<IpollService, PollService>();

        //add mapster
        services.AddMapSterServices();

        //add fluent validation
        services.AddFluentValidation();


        return services;
    }

    private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();     

        return services;
    }

    private static IServiceCollection AddMapSterServices(this IServiceCollection services)
    {
       
        var mappingconfig = TypeAdapterConfig.GlobalSettings;
        mappingconfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingconfig));

        return services;
    }
    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.
              AddFluentValidationAutoValidation()
              .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;

    }
}
