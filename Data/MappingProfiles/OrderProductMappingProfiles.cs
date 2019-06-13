using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.OrderProduct;
using AllNewsServer.Data.Models;

public class OrderProductMappingProfiles : Profile {
  public OrderProductMappingProfiles () {
    CreateMap<OrderProduct, OrderProductDTO> ();
    CreateMap<AddOrderProductDTO, OrderProduct> ();
  }
}