using System;
using System.Linq;
using AutoMapper;
using Tenli.Server.Data.DTOs.ActiveSession;
using Tenli.Server.Data.Models;

public class ActiveSessionMappingProfiles : Profile {
  public ActiveSessionMappingProfiles () {
    CreateMap<ActiveSession, ActiveSessionDTO> ();
  }
}