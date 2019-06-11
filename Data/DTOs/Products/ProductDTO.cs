using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Currency;
using Tenli.Server.Data.DTOs.Order;
using Tenli.Server.Data.DTOs.ProductType;
using Tenli.Server.Data.DTOs.SizeUnit;
using Tenli.Server.Data.DTOs.VolumeUnit;
using Tenli.Server.Data.DTOs.WeightUnit;

namespace Tenli.Server.Data.DTOs.Product {
  public class ProductDTO:ProductWithoutProductsDTO {
    public virtual List<OrderDTO> Orders { get; set; }
  }
}