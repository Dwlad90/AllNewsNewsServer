using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Currency;
using AllNewsServer.Data.DTOs.Order;
using AllNewsServer.Data.DTOs.ProductType;
using AllNewsServer.Data.DTOs.SizeUnit;
using AllNewsServer.Data.DTOs.VolumeUnit;
using AllNewsServer.Data.DTOs.WeightUnit;

namespace AllNewsServer.Data.DTOs.Product {
  public class ProductDTO:ProductWithoutProductsDTO {
    public virtual List<OrderDTO> Orders { get; set; }
  }
}