using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Order;

namespace Tenli.Server.Data.DTOs.DeliveryType {
  public class DeliveryTypeDTO : DeliveryTypeWithoutOrdersDTO {
    public virtual ICollection<OrderDTO> Orders { get; set; }
  }
}