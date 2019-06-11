using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.Product;
using Tenli.Server.Data.Models;

public class ProductMappingProfiles : Profile {
  public ProductMappingProfiles () {
    CreateMap<AddProductDTO, Product> ();
    CreateMap<Product, ProductDTO> ()
      .ForMember (dest => dest.Orders, opt => opt.MapFrom (src => src.OrderProducts.Select (x => x.Order)));
    CreateMap<Product, ProductWithoutProductsDTO> ();
  }
}