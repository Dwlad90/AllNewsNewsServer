using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.VolumeUnit {
  public class VolumeUnitDTO :VolumeUnitWithoutProductsDTO {
    public List<ProductDTO> Products { get; set; }
  }
}