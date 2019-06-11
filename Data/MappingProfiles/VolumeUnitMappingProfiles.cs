using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.VolumeUnit;
using Tenli.Server.Data.Models;

public class VolumeUnitMappingProfiles : Profile {
  public VolumeUnitMappingProfiles () {
    CreateMap<VolumeUnit, VolumeUnitDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
    CreateMap<VolumeUnit, VolumeUnitWithoutProductsDTO> ()
      .ForMember (dest => dest.Description, opt => opt.MapFrom (src => src.Descriptions.FirstOrDefault ().Value));
  }
}