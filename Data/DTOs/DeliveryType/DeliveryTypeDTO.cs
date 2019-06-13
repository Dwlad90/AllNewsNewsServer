using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Order;

namespace AllNewsServer.Data.DTOs.DeliveryType {
  public class DeliveryTypeDTO : DeliveryTypeWithoutOrdersDTO {
    public virtual ICollection<OrderDTO> Orders { get; set; }
  }
}