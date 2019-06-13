using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AllNewsServer.Services.Extensions {
  public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterServices (
      this IServiceCollection services) {
      services.AddTransient<ApplicationUserService> ();
      services.AddTransient<ApplicationRoleService> ();
      services.AddTransient<NewsService> ();
      services.AddTransient<DbContextService> ();

      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor> ();

      var scopeFactory = services
        .BuildServiceProvider ()
        .GetRequiredService<IServiceScopeFactory> ();

      return services;
    }
  }
}