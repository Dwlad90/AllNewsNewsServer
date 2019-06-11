using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;
using System.Threading;
using System.Web;
using Tenli.Server.Services;


namespace Tenli.Server.Services
{
    public class DbContextService
    {
        private ApplicationDbContext db;
        private readonly IHttpContextAccessor _accessor;
        public DbContextService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void AddBaseEntityProperties(ChangeTracker changeTracker)
        {
            var entities = changeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            if (entities.Count() == 0)
            {
                return;
            }

            string userEmail = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;
            DateTime now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreationDateTime = now;
                    ((BaseEntity)entity.Entity).CreationUser = userEmail;
                }

                ((BaseEntity)entity.Entity).ModificationDateTime = now;
                ((BaseEntity)entity.Entity).ModificationUser = userEmail;
            }
        }
    }
}