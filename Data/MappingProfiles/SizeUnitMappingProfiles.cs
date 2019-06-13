using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.SizeUnit;
using AllNewsServer.Data.Models;

public class SizeUnitMappingProfiles : Profile {
  public SizeUnitMappingProfiles () {
    CreateMap<SizeUnit, SizeUnitDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
       CreateMap<SizeUnit, SizeUnitWithoutProductsDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
  }
}