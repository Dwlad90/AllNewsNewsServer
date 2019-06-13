using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AllNewsServer.Data.DTOs.ApplicationRole;

namespace AllNewsServer.Data.DTOs.ApplicationUser
{
    public class GetTokenApplicationUserDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string RefreshToken { get; set; }

    }
}

