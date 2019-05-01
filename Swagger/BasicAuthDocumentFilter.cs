using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace BasicAuthenticationMiddleware
{
    public class BasicAuthDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var securityRequirements = new Dictionary<string, IEnumerable<string>>()
            {
                { "basic", new string[] { } } 
            };

            swaggerDoc.Security = new[] { securityRequirements };
        }
    }
}
