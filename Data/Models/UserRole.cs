using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tenli.Server.Data.Models
{
    public class UserRole
    {
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int ApplicationRoleId { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
