using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.OrderStatus;
using AllNewsServer.Data.Models;

public class OrderStatusMappingProfiles : Profile {
  public OrderStatusMappingProfiles () {
    CreateMap<OrderStatus, OrderStatusDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
    CreateMap<OrderStatus, OrderStatusWithoutOrdersDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
  }
}