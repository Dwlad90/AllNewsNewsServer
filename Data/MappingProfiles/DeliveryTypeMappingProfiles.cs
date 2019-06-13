using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.DeliveryType;
using AllNewsServer.Data.Models;

public class DeliveryTypeMappingProfiles : Profile {
  public DeliveryTypeMappingProfiles () {
    CreateMap<DeliveryType, DeliveryTypeDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
    CreateMap<DeliveryType, DeliveryTypeWithoutOrdersDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
  }
}