using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.WeightUnit {
  public class WeightUnitDTO :WeightUnitWithoutProductsDTO{
    public virtual List<ProductDTO> Products { get; set; }
  }
}