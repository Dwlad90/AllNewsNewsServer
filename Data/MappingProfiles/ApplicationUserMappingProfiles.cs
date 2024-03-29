using System;
using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.ApplicationUser;
using AllNewsServer.Data.Models;

public class ApplicationUserMappingProfiles : Profile {
  public ApplicationUserMappingProfiles () {
    CreateMap<ApplicationUser, ApplicationUserDTO> ()
      .ForMember (dest => dest.Roles, opt => opt.MapFrom (src => src.UserRoles.Select (x => x.ApplicationRole)));

    CreateMap<ApplicationUser, GetTokenApplicationUserDTO> ();
    CreateMap<AddApplicationUserDTO, ApplicationUser> ();
    CreateMap<SignupApplicationUserDTO, ApplicationUser> ();
    CreateMap<UpdateApplicationUserDTO, ApplicationUser> ();
  }
}