using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.WeightUnit {
  public class WeightUnitDTO :WeightUnitWithoutProductsDTO{
    public virtual List<ProductDTO> Products { get; set; }
  }
}