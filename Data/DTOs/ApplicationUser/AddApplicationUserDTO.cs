using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AllNewsServer.Data.DTOs.ApplicationRole;

namespace AllNewsServer.Data.DTOs.ApplicationUser
{
    public class AddApplicationUserDTO
    {
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

    }
}

