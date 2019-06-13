using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Order;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.OrderProduct
{
    public class AddOrderProductDTO
    {
        public int OrderId { get; set; }
        public virtual AddOrderDTO Order { get; set; }
        public int ProductId { get; set; }
        public virtual AddProductDTO Product { get; set; }
    }
}
