using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.WeightUnit;
using AllNewsServer.Data.Models;

public class WeightUnitMappingProfiles : Profile {
  public WeightUnitMappingProfiles () {
    CreateMap<WeightUnit, WeightUnitDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
    CreateMap<WeightUnit, WeightUnitWithoutProductsDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
  }
}