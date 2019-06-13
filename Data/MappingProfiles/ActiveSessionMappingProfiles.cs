using System;
using System.Linq;
using AutoMapper;
using AllNewsServer.Data.DTOs.ActiveSession;
using AllNewsServer.Data.Models;

public class ActiveSessionMappingProfiles : Profile {
  public ActiveSessionMappingProfiles () {
    CreateMap<ActiveSession, ActiveSessionDTO> ();
  }
}