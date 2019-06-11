using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Tenli.Server.Data.Models {
  public class Order : BaseEntity {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int OrderStatusId { get; set; }
    public virtual OrderStatus OrderStatus { get; set; }
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    public DateTime ExpectingDateTime { get; set; }
    public DateTime? FinishingTime { get; set; }
    public DateTime? TakingDateTime { get; set; }

    [Column (TypeName = "geography")]
    public Point StartLocation { get; set; }

    [Column (TypeName = "geography")]
    public Point EndLocation { get; set; }

    public int DeliveryTypeId { get; set; }
    public virtual DeliveryType DeliveryType { get; set; }
    public int CustomerId { get; set; }
    public virtual ApplicationUser Customer { get; set; }
    public int? ExecutorId { get; set; }
    public virtual ApplicationUser Executor { get; set; }
  }
}