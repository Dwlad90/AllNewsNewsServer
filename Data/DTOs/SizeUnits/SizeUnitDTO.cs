using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.SizeUnit {
  public class SizeUnitDTO:SizeUnitWithoutProductsDTO {
    public List<ProductDTO> Products { get; set; }
  }
}