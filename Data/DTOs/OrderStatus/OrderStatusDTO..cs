using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Order;

namespace AllNewsServer.Data.DTOs.OrderStatus {
  public class OrderStatusDTO : OrderStatusWithoutOrdersDTO {
    public virtual ICollection<OrderDTO> Orders { get; set; }
  }
}