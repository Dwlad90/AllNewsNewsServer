using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Order;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.OrderProduct
{
    public class OrderProductDTO
    {
        public int OrderId { get; set; }
        public virtual OrderDTO Order { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
    }
}
