﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tenli.Server.Data;
using Tenli.Server.Services;

namespace Tenli.Server {
  public class Program {
    public static void Main (string[] args) {
      var host = CreateWebHostBuilder (args).Build ();

      using (var scope = host.Services.CreateScope ()) {
        var services = scope.ServiceProvider;
        try {
          var context = services.GetRequiredService<ApplicationDbContext> ();
          var inMemoryDbService = services.GetRequiredService<RedisService> ();
          DbInitializer.Seed (context, inMemoryDbService);
        } catch (Exception ex) {
          var logger = services.GetRequiredService<ILogger<Program>> ();
          logger.LogError (ex, "An error occurred while seeding the database.");
        }
      }

      host.Run ();
    }

    public static IWebHostBuilder CreateWebHostBuilder (string[] args) {
      var config = new ConfigurationBuilder ().AddCommandLine (args).Build ();
      return WebHost.CreateDefaultBuilder (args)
        .UseConfiguration (config)
        .UseStartup<Startup> ();
    }
  }
}