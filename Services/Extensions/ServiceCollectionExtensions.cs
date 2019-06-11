using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tenli.Server.Services.Extensions {
  public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterServices (
      this IServiceCollection services) {
      services.AddTransient<ApplicationUserService> ();
      services.AddTransient<ApplicationRoleService> ();
      services.AddTransient<CultureService> ();
      services.AddTransient<DbContextService> ();
      services.AddTransient<RedisService> ();
      services.AddTransient<OrderStatusService> ();
      services.AddTransient<OrderService> ();
      services.AddTransient<ProductService> ();
      services.AddTransient<RedisKeysService> ();

      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor> ();

      var scopeFactory = services
        .BuildServiceProvider ()
        .GetRequiredService<IServiceScopeFactory> ();

      return services;
    }
  }
}