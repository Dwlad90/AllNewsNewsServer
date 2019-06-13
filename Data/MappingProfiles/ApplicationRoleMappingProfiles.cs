using System;
using AutoMapper;
using AllNewsServer.Data.DTOs.ApplicationRole;
using AllNewsServer.Data.Models;

public class ApplicationRoleMappingProfiles : Profile {
  public ApplicationRoleMappingProfiles () {
    CreateMap<ApplicationRole, ApplicationRoleDTO> ();
  }
}