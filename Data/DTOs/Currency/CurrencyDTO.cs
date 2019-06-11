using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.Currency {
  public class CurrencyDTO : CurrencyWithoutProductsDTO {
    public List<ProductDTO> Products { get; set; }
  }
}