using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.WeightUnit;
using Tenli.Server.Data.Models;

public class WeightUnitMappingProfiles : Profile {
  public WeightUnitMappingProfiles () {
    CreateMap<WeightUnit, WeightUnitDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
    CreateMap<WeightUnit, WeightUnitWithoutProductsDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
  }
}