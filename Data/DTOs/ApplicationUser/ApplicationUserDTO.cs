using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tenli.Server.Data.DTOs.ApplicationRole;

namespace Tenli.Server.Data.DTOs.ApplicationUser
{
    public class ApplicationUserDTO
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsBanned { get; set; }

        public double Balance { get; set; }

        public double Hold { get; set; }        
        
        public double Rating { get; set; }

        public virtual IEnumerable<ApplicationRoleDTO> Roles { get; set; }
    }
}

