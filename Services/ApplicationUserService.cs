using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Tenli.Server.Services
{
    public class ApplicationUserService
    {
        private ApplicationDbContext db;

        public ApplicationUserService(ApplicationDbContext context)
        {
            db = context;
        }

        public Task<ApplicationUser> GetApplicationUserAsync(int userId)
        {
            return db.ApplicationUsers
                .Include(x => x.UserRoles).ThenInclude(x => x.ApplicationRole)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }

        public ApplicationUser GetApplicationUser(int userId)
        {
            return db.ApplicationUsers
                .Include(x => x.UserRoles).ThenInclude(x => x.ApplicationRole)
                .SingleOrDefault(x => x.Id == userId);
        }

        public Task<List<ApplicationUser>> GetApplicationUsersByIdsAsync(List<int> userIds)
        {
            return db.ApplicationUsers.Where(x => userIds.Contains(x.Id)).ToListAsync();
        }

        public Task<ApplicationUser> GetApplicationUserWithSessionsAsync(int userId)
        {
            return db.ApplicationUsers
                .Include(x => x.ActiveSessions)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }

        public Task<List<ActiveSession>> GetUserActiveSessions(int userId)
        {
            return db.ActiveSessions
                .Where(x => x.ApplicationUserId == userId && !x.IsTerminated)
                .OrderByDescending(x => x.LastRefreshDateTime)
                .ToListAsync();
        }

        public Task<List<ApplicationUser>> GetApplicationUsersAsync(string query, int offset, int limit)
        {
            var queryable = db.ApplicationUsers.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                var searchOpts = query.Split(' ').ToList();

                queryable = queryable.Where(x =>
                     searchOpts.All(y =>
                     x.Phone.Contains(y)));
            }

            return queryable
                .OrderBy(x => x.Phone)
                .Skip(offset).Take(limit)
                .Include(x => x.UserRoles).ThenInclude(x => x.ApplicationRole)
                .ToListAsync();
        }

        public Task<List<ApplicationUser>> GetApplicationUsersWithPushNotificationTokenAsync(string query, int offset, int limit)
        {
            var queryable = db.ApplicationUsers.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                var searchOpts = query.Split(' ').ToList();

                queryable = queryable.Where(x =>
                     searchOpts.All(y =>
                     x.Phone.Contains(y)));
            }

            return queryable
                .Where(x => !string.IsNullOrEmpty(x.PushNotificationsToken))
                .OrderBy(x => x.Phone)
                .Skip(offset).Take(limit)
                .ToListAsync();
        }

        public Task<List<ApplicationUser>> GetApplicationUsersInRoleAsync(string query, string role, int offset, int limit)
        {
            var queryable = db.ApplicationUsers
                .Where(x => x.UserRoles.Any(r => r.ApplicationRole.Name == role))
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                var searchOpts = query.Split(' ').ToList();

                queryable = queryable.Where(x =>
                     searchOpts.All(y =>
                     x.Phone.Contains(y)));
            }

            return queryable
                .OrderBy(x => x.Phone)
                .Skip(offset).Take(limit)
                .Include(x => x.UserRoles).ThenInclude(x => x.ApplicationRole)
                .ToListAsync();
        }

        public Task<ApplicationUser> GetUserByPhoneAsync(string phone)
        {
            return db.ApplicationUsers
                .SingleOrDefaultAsync(x => x.Phone == phone);
        }

        public Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return db.ApplicationUsers
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task<ApplicationUser> GetUserByPhoneWithSessionsAsync(string phone)
        {
            return db.ApplicationUsers
                .Include(x => x.ActiveSessions)
                .SingleOrDefaultAsync(x => x.Phone == phone);
        }

        public Task<ApplicationUser> GetUserWithRolesByEmailAsync(string email)
        {
            return db.ApplicationUsers
                .Include(x => x.UserRoles).ThenInclude(x => x.ApplicationRole)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task AddApplicationUserAsync(ApplicationUser user)
        {
            db.ApplicationUsers.Add(user);
            return db.SaveChangesAsync();
        }

        public Task AddMultipleApplicationUsersAsync(List<ApplicationUser> users)
        {
            db.ApplicationUsers.AddRange(users);
            return db.SaveChangesAsync();
        }

        public Task UpdateApplicationUserAsync(ApplicationUser user)
        {
            db.Entry<ApplicationUser>(user).State = EntityState.Modified;
            return db.SaveChangesAsync();
        }

        public Task UpdateMultipleApplicationUserAsync(List<ApplicationUser> users)
        {
            users.ForEach(user =>
            {
                db.Entry<ApplicationUser>(user).State = EntityState.Modified;
            });

            return db.SaveChangesAsync();
        }

        public Task<bool> Exists(int userId)
        {
            return db.ApplicationUsers.AnyAsync(x => x.Id == userId);
        }
    }
}