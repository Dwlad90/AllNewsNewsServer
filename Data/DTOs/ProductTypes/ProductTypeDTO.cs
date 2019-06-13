using System.Collections.Generic;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.ProductType {
  public class ProductTypeDTO:ProductTypeWithoutProductsDTO{
    public List<ProductDTO> Products { get; set; }
  }
}