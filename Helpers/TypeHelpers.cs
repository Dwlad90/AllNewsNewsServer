using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tenli.Server.Data.Constants;

namespace Tenli.Server.Helpers {
  public class TypeHelpers {
    public static List<KeyValuePair<string, string>> getTypeValues (Type type) {
      Dictionary<string, string> types = new Dictionary<string, string> ();

      foreach (var property in type.GetFields ()) {
        string value = property.GetValue (null).ToString ();
        string key = property.Name;
        types.Add (key:key, value:value);
      }

      return types.ToList ();
    }
  }
}