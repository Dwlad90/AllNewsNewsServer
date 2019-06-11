using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.ProductType;
using Tenli.Server.Data.Models;

public class ProductTypeMappingProfiles : Profile {
  public ProductTypeMappingProfiles () {
    CreateMap<ProductType, ProductTypeDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
    CreateMap<ProductType, ProductTypeWithoutProductsDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
  }
}