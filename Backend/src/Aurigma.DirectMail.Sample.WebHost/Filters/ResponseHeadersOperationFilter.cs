using System.Collections.Generic;
using System.Linq;
using Aurigma.DirectMail.Sample.WebApi.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Aurigma.DirectMail.Sample.WebHost.Filters;

/// <summary>
/// Swagger operation filter which is used to add the description of produced response headers to swagger document.
/// </summary>
public class ResponseHeadersOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType == null)
            return;

        var actionResponsesWithHeaders = context
            .MethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<ProducesResponseHeaderAttribute>()
            .ToArray();

        if (!actionResponsesWithHeaders.Any())
            return;

        foreach (var responseCode in operation.Responses.Keys)
        {
            var responseHeaders = actionResponsesWithHeaders
                .Where(x => x.StatusCode.ToString() == responseCode)
                .ToArray();

            if (!responseHeaders.Any())
                continue;

            var response = operation.Responses[responseCode];
            response.Headers ??= new Dictionary<string, OpenApiHeader>();

            foreach (var responseHeader in responseHeaders)
            {
                response.Headers[responseHeader.Name] = new OpenApiHeader
                {
                    Schema = new OpenApiSchema { Type = responseHeader.Type.ToString().ToLower() },
                    Description = responseHeader.Description,
                };
            }
        }
    }
}
