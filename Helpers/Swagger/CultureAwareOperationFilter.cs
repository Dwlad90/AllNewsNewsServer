using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using AllNewsServer.Data.Constants;

namespace AllNewsServer.Helpers.Swagger {
  /// <summary>
  /// Operation filter to add the requirement of the custom header
  /// </summary>
  public class AcceptLanguageHeaderFilter : IOperationFilter {
    public void Apply (Operation operation, OperationFilterContext context) {
      if (operation.Parameters == null)
        operation.Parameters = new List<IParameter> ();

      operation.Parameters.Add (new NonBodyParameter {
        Name = "Accept-Language",
          In = "header",
          Type = "string",
          Required = true, // set to false if this is optional
          Enum = TypeHelpers.getTypeValues(typeof(CultureKeys)).Select(x => x.Value).ToArray(),
          Default = "en"
      });
    }
  }
}