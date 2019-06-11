using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.LocalizationResource;
using Tenli.Server.Data.Models;

public class LocalizationResourceMappingProfiles : Profile {
  public LocalizationResourceMappingProfiles () {
    CreateMap<LocalizationResource, LocalizationResourceDTO> ();
  }
}