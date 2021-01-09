namespace AdsPortal.WebApi.Rest.Configurations.SwaggerFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    //Filter for public async Task<IActionResult> PostFormData([FromForm] IFormFile file)
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!(operation.RequestBody?.Content?.Any(x => x.Key == "multipart/form-data") ?? false))
            {
                return;
            }

            OpenApiParameter? fileParameter = operation.Parameters.FirstOrDefault(x => x.Name == "file");
            if (fileParameter != null)
            {
                operation.Parameters.Remove(fileParameter);
            }

            IList<ParameterDescriptor> parameters = context.ApiDescription.ActionDescriptor.Parameters;
            ParameterDescriptor? parameterDescriptor = parameters.Where(x => x.ParameterType == typeof(IFormFile)).FirstOrDefault();

            if (parameterDescriptor is null)
            {
                return;
            }

            string parameterName = parameterDescriptor.Name;

            OpenApiMediaType uploadFileMediaType = new OpenApiMediaType()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties =
                    {
                        [parameterName] = new OpenApiSchema()
                        {
                            Description = "Upload File",
                            Type = "file",
                            Format = "formData",
                            MultipleOf = 10
                        }
                    },
                    Required = new HashSet<string>() { parameterName }
                }
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = uploadFileMediaType }
            };
        }
    }
}
