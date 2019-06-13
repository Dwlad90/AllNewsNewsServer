using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.Currency {
  public class CurrencyDTO : CurrencyWithoutProductsDTO {
    public List<ProductDTO> Products { get; set; }
  }
}