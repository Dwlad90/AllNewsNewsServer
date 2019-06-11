using System;
using System.Collections.Generic;
using Tenli.Server.Data.DTOs.ApplicationUser;
using Tenli.Server.Data.DTOs.OrderProduct;
using Tenli.Server.Data.DTOs.OrderStatus;
using Tenli.Server.Data.DTOs.Product;
using Tenli.Server.Data.DTOs.DeliveryType;

namespace Tenli.Server.Data.DTOs.Order {
  public class OrderDTO {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual OrderStatusWithoutOrdersDTO OrderStatus { get; set; }
    public List<ProductWithoutProductsDTO> Products { get; set; }
    public DateTime ExpectingDateTime { get; set; }
    public DateTime? FinishingTime { get; set; }
    public DateTime? TakingDateTime { get; set; }
    public double[] StartLocation { get; set; }
    public double[] EndLocation { get; set; }
    public virtual ApplicationUserDTO Customer { get; set; }
    public virtual ApplicationUserDTO Executor { get; set; }
    public virtual DeliveryTypeWithoutOrdersDTO DeliveryType { get; set; }
  }
}