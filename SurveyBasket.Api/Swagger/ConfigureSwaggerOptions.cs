using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SurveyBasket.Api.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));


    }
    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description) =>
        new()
        {
            Title = "Survey Basket API",
            Version = description.ApiVersion.ToString(),
            Description = $"API Description.{(description.IsDeprecated ? "This Api Has been Deprecated." : string.Empty)}"
        };


}
