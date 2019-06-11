using System;
using AutoMapper;
using Tenli.Server.Data.DTOs.ApplicationRole;
using Tenli.Server.Data.Models;

public class ApplicationRoleMappingProfiles : Profile {
  public ApplicationRoleMappingProfiles () {
    CreateMap<ApplicationRole, ApplicationRoleDTO> ();
  }
}