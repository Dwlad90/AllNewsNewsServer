using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tenli.Server.Data.Constants;

namespace Tenli.Server.Helpers {
  public class LocationHelpers {
    public static bool ComparePosition (Point mainPoint, Point comparedPoint, double radius) {
      return mainPoint.Distance (comparedPoint) < radius / (double) (LocationEnum.DistanceToKM);
    }
  }
}