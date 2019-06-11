using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Order;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.OrderProduct
{
    public class OrderProductDTO
    {
        public int OrderId { get; set; }
        public virtual OrderDTO Order { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
    }
}
