using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.DeliveryType;
using Tenli.Server.Data.Models;

public class DeliveryTypeMappingProfiles : Profile {
  public DeliveryTypeMappingProfiles () {
    CreateMap<DeliveryType, DeliveryTypeDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
    CreateMap<DeliveryType, DeliveryTypeWithoutOrdersDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
  }
}