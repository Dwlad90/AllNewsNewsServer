using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.LocalizationResource;
using AllNewsServer.Data.Models;

public class LocalizationResourceMappingProfiles : Profile {
  public LocalizationResourceMappingProfiles () {
    CreateMap<LocalizationResource, LocalizationResourceDTO> ();
  }
}