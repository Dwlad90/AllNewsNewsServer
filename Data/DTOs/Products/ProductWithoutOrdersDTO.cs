using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Currency;
using AllNewsServer.Data.DTOs.Order;
using AllNewsServer.Data.DTOs.ProductType;
using AllNewsServer.Data.DTOs.SizeUnit;
using AllNewsServer.Data.DTOs.VolumeUnit;
using AllNewsServer.Data.DTOs.WeightUnit;

namespace AllNewsServer.Data.DTOs.Product {
  public class ProductWithoutProductsDTO {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public virtual ProductTypeWithoutProductsDTO ProductType { get; set; }
    public double Weight { get; set; }
    public virtual WeightUnitWithoutProductsDTO WeightUnit { get; set; }
    public double? Volume { get; set; }
    public virtual VolumeUnitWithoutProductsDTO VolumeUnit { get; set; }
    public double? Height { get; set; }
    public double?  Width { get; set; }
    public double? Depth { get; set; }
    public virtual SizeUnitWithoutProductsDTO SizeUnit { get; set; }
    public double Price { get; set; }
    public virtual CurrencyWithoutProductsDTO Currency { get; set; }
  }
}