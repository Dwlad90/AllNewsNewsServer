using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AllNewsServer.Data.DTOs.ApplicationRole;

namespace AllNewsServer.Data.DTOs.ApplicationUser
{
    public class SignupApplicationUserDTO:AddApplicationUserDTO
    {
        [MinLength(6), Required]
        public string Password { get; set; }
    }
}

