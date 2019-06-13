using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AllNewsServer.Data.Constants;
using AllNewsServer.Data.Models;
using AllNewsServer.Helpers;
using AllNewsServer.Services;
using Microsoft.EntityFrameworkCore;
using SalesAudit.Data.Constants;

namespace AllNewsServer.Data {
  public class DbInitializer {
    public static void Seed (ApplicationDbContext context) {
      Console.WriteLine ("Performing database initializaton...");
      Stopwatch stopWatch = Stopwatch.StartNew ();

      try {
        context.Database.EnsureCreated ();

        AddOrUpdateUsers (context);
        AddOrUpdateRoles (context);

        context.SaveChanges ();

        ApplyAdminRoles (context);

        context.SaveChanges ();
      } catch (Exception e) {
        Console.WriteLine ("Failed to initialize database. Ignore this if you are running 'migrations add' command");
        System.Console.WriteLine (e);
      }

      Console.WriteLine ("Database initialized: elapsed time {0} ms", stopWatch.ElapsedMilliseconds);
    }

    private static void AddOrUpdateUsers (ApplicationDbContext context) {
      var users = new List<ApplicationUser> () {
        new ApplicationUser { Phone = "972532403308", Email = "dwlad90@gmail.com", Password = "password" }
      };

      foreach (var user in users) {
        var exists = context.ApplicationUsers.Any (x => x.Phone == user.Phone);

        if (!exists) {
          context.ApplicationUsers.Add (user);
        }
      }
    }

    private static void AddOrUpdateRoles (ApplicationDbContext context) {
      var roles = new List<ApplicationRole> () {
        new ApplicationRole { Name = ApplicationRoleNames.Admin, Description = "Администратор системы" },
        new ApplicationRole { Name = ApplicationRoleNames.TopManager, Description = "Менеджер, который имеет доступ ко всем проектам" },
        new ApplicationRole { Name = ApplicationRoleNames.Manager, Description = "Менеджер, который имеет доступ только к своим проектам" },
        new ApplicationRole { Name = ApplicationRoleNames.Operator, Description = "Оператор, проверяющий задания, выполненные пользователями мобильного приложения" },
      };

      foreach (var role in roles) {
        var exists = context.ApplicationRoles.Any (x => x.Name == role.Name);

        if (!exists) {
          context.ApplicationRoles.Add (role);
        }
      }
    }

    private static void ApplyAdminRoles (ApplicationDbContext context) {
      var administrator = context.ApplicationUsers
        .Include (x => x.UserRoles)
        .ThenInclude (x => x.ApplicationRole)
        .SingleOrDefault (x => x.Phone == "972532403308");

      if (administrator != null) {
        var isAdmin = administrator.UserRoles.Any (x => x.ApplicationRole.Name == "Admin");

        if (!isAdmin) {
          var adminRole = context.ApplicationRoles.SingleOrDefault (x => x.Name == "Admin");

          if (adminRole != null)
            administrator.UserRoles.Add (new UserRole { ApplicationRoleId = adminRole.Id });
        }
      }
    }
    private static void AddOrUpdateCultures (ApplicationDbContext context) {
      TypeHelpers.getTypeValues (typeof (CultureKeys)).ForEach ((type) => {
        Culture culture = new Culture { Key = type.Value, Description = type.Key };

        var item = context.Cultures.FirstOrDefault (x => x.Key == type.Value);

        if (item == null) {
          context.Cultures.Add (culture);
          item = culture;
        }

      });
    }
  }
}