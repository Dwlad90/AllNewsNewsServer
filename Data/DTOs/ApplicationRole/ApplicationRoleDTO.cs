using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllNewsServer.Data.DTOs.ApplicationRole
{
    public class ApplicationRoleDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}

