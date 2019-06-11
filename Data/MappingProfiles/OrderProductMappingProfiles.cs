using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.OrderProduct;
using Tenli.Server.Data.Models;

public class OrderProductMappingProfiles : Profile {
  public OrderProductMappingProfiles () {
    CreateMap<OrderProduct, OrderProductDTO> ();
    CreateMap<AddOrderProductDTO, OrderProduct> ();
  }
}