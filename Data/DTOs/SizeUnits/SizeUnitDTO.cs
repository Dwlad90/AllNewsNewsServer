using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.SizeUnit {
  public class SizeUnitDTO:SizeUnitWithoutProductsDTO {
    public List<ProductDTO> Products { get; set; }
  }
}