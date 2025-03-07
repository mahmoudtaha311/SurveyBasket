using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SurveyBasket.Api.Swagger;

public class SwaggerDefaultValues : IOperationFilter
{

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated();
        if (operation.Parameters == null)
        {
            return;
        }

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions
                .First(p => p.Name == parameter.Name);
            if (parameter.Schema.Default is null && description.DefaultValue is not null)
            {
                var json = JsonSerializer.Serialize(
                    description.DefaultValue,
                    description.ModelMetadata!.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }
            parameter.Required |= description.IsRequired;
        }
    }
}
