using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AllNewsServer.Data.DTOs.ApplicationRole;

namespace AllNewsServer.Data.DTOs.ApplicationUser
{
    public class TerminateSessionDTO
    {
        public string RefreshToken { get; set; }
    }
}

