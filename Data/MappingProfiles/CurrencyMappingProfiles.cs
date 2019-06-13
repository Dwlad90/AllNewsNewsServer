using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.Currency;
using AllNewsServer.Data.Models;

public class CurrencyMappingProfiles : Profile {
  public CurrencyMappingProfiles () {
    CreateMap<Currency, CurrencyDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
    CreateMap<Currency, CurrencyWithoutProductsDTO> ()
      .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Names.FirstOrDefault ().Value));
  }
}