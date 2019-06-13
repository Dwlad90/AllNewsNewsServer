using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using AllNewsServer.Data;
using AllNewsServer.Data.Constants;
using AllNewsServer.Data.Models;

namespace AllNewsServer.Services
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