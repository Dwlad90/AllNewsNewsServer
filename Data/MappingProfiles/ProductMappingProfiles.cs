using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.Product;
using AllNewsServer.Data.Models;

public class ProductMappingProfiles : Profile {
  public ProductMappingProfiles () {
    CreateMap<AddProductDTO, Product> ();
    CreateMap<Product, ProductDTO> ()
      .ForMember (dest => dest.Orders, opt => opt.MapFrom (src => src.OrderProducts.Select (x => x.Order)));
    CreateMap<Product, ProductWithoutProductsDTO> ();
  }
}