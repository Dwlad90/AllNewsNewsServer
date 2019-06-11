using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Order;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.OrderProduct
{
    public class AddOrderProductDTO
    {
        public int OrderId { get; set; }
        public virtual AddOrderDTO Order { get; set; }
        public int ProductId { get; set; }
        public virtual AddProductDTO Product { get; set; }
    }
}
