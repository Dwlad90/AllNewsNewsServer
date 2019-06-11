using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Tenli.Server.Data.Models {
  public class Product : BaseEntity {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int ProductTypeId { get; set; }
    public virtual ProductType ProductType { get; set; }
    public double Weight { get; set; }
    public int WeightUnitId { get; set; }
    public virtual WeightUnit WeightUnit { get; set; }
    public double? Volume { get; set; }
    public int? VolumeUnitId { get; set; }
    public virtual VolumeUnit VolumeUnit { get; set; }
    public double? Height { get; set; }
    public double?  Width { get; set; }
    public double? Depth { get; set; }
    public int? SizeUnitId { get; set; }
    public virtual SizeUnit SizeUnit { get; set; }
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    public double Price { get; set; }
    public int? CurrencyId { get; set; }
    public virtual Currency Currency { get; set; }
  }
}