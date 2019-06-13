using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.Culture;
using AllNewsServer.Data.Models;

public class CultureMappingProfiles : Profile {
  public CultureMappingProfiles () {
    CreateMap<Culture, CultureDTO> ();
  }
}