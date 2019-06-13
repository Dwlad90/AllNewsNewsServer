using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.Order;
using AllNewsServer.Data.DTOs.OrderProduct;
using AllNewsServer.Data.Models;

public class OrderMappingProfiles : Profile {
  public OrderMappingProfiles () {
    CreateMap<AddOrderDTO, Order> ().ForMember (dest => dest.OrderProducts, opt => opt.MapFrom (src => src.Products.Select (x => new AddOrderProductDTO { Order = src, Product = x }).ToList ()));
    CreateMap<Order, OrderDTO> ()
      .ForMember (dest => dest.StartLocation, opt => opt.MapFrom (src => new [] { src.StartLocation.X, src.StartLocation.Y }))
      .ForMember (dest => dest.EndLocation, opt => opt.MapFrom (src => new [] { src.EndLocation.X, src.EndLocation.Y }))
      .ForMember (dest => dest.Products, opt => opt.MapFrom (src => src.OrderProducts.Select (x => x.Product)));
    CreateMap<UpdateOrderDTO, Order> ();
  }
}