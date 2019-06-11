using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;

namespace Tenli.Server.Services
{
    public class ApplicationRoleService
    {
        private ApplicationDbContext db;

        public ApplicationRoleService(ApplicationDbContext context)
        {
            db = context;
        }

        public Task<List<ApplicationRole>> GetApplicationRolesAsync()
        {
            return db.ApplicationRoles.ToListAsync();
        }
    }
}