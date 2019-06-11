using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Order;

namespace Tenli.Server.Data.DTOs.OrderStatus {
  public class OrderStatusDTO : OrderStatusWithoutOrdersDTO {
    public virtual ICollection<OrderDTO> Orders { get; set; }
  }
}