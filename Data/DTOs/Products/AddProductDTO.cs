using System.Collections.Generic;
namespace AllNewsServer.Data.DTOs.Product {
  public class AddProductDTO {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int ProductTypeId { get; set; }
    public double Weight { get; set; }
    public int WeightUnitId { get; set; }
    public double? Volume { get; set; }
    public int? VolumeUnitId { get; set; }
    public double? Height { get; set; }
    public double?  Width { get; set; }
    public double? Depth { get; set; }
    public int? SizeUnitId { get; set; }
    public double Price { get; set; }
    public int CurrencyId { get; set; }
  }
}