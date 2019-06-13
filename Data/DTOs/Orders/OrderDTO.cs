using System;
using System.Collections.Generic;
using AllNewsServer.Data.DTOs.ApplicationUser;
using AllNewsServer.Data.DTOs.OrderProduct;
using AllNewsServer.Data.DTOs.OrderStatus;
using AllNewsServer.Data.DTOs.Product;
using AllNewsServer.Data.DTOs.DeliveryType;

namespace AllNewsServer.Data.DTOs.Order {
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