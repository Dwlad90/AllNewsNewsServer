using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Product;

namespace Tenli.Server.Data.DTOs.ProductType {
  public class ProductTypeDTO:ProductTypeWithoutProductsDTO{
    public List<ProductDTO> Products { get; set; }
  }
}