using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.SizeUnit;
using Tenli.Server.Data.Models;

public class SizeUnitMappingProfiles : Profile {
  public SizeUnitMappingProfiles () {
    CreateMap<SizeUnit, SizeUnitDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
       CreateMap<SizeUnit, SizeUnitWithoutProductsDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
  }
}