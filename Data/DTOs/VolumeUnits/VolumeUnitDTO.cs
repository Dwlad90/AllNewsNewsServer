using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.VolumeUnit {
  public class VolumeUnitDTO :VolumeUnitWithoutProductsDTO {
    public List<ProductDTO> Products { get; set; }
  }
}