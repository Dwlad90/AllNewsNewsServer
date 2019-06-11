using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.Culture;
using Tenli.Server.Data.Models;

public class CultureMappingProfiles : Profile {
  public CultureMappingProfiles () {
    CreateMap<Culture, CultureDTO> ();
  }
}