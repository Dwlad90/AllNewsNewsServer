using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Swashbuckle.AspNetCore.Swagger;
using Tenli.Server.Configurations;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Helpers;
using Tenli.Server.Helpers.Swagger;
using Tenli.Server.Services;
using Tenli.Server.Services.Extensions;
namespace Tenli.Server {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      services.AddDbContext<ApplicationDbContext> (options =>
        options.UseNpgsql (Configuration.GetConnectionString ("DefaultConnection"), x => x.UseNetTopologySuite ()));

      services
        .AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer (options => {
          //options.RequireHttpsMetadata = true; // commented out for development
          options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = AuthConfiguration.TOKEN_ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthConfiguration.TOKEN_AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthConfiguration.GetSymmetricSecurityKey (),
            ValidateIssuerSigningKey = true
          };
        });

      services.AddAutoMapper ();

      services.RegisterServices ();

      services.Configure<RequestLocalizationOptions> (
        options => {
          List<CultureInfo> supportedCultures = TypeHelpers.getTypeValues (typeof (CultureKeys)).Select (x => new CultureInfo (x.Value)).ToList ();

          options.DefaultRequestCulture = new RequestCulture (culture: CultureKeys.English, uiCulture: CultureKeys.English);
          options.SupportedCultures = supportedCultures;
          options.SupportedUICultures = supportedCultures;
        });

      services.AddMvc ()
        .AddViewLocalization ()
        .AddDataAnnotationsLocalization ()
        .SetCompatibilityVersion (CompatibilityVersion.Version_2_2)
        .AddJsonOptions (options => {
          options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        });

      // Register the Swagger generator, defining 1 or more Swagger documents
      services.AddSwaggerGen (c => {
        c.SwaggerDoc ("v1", new Info { Title = "Tenli API", Version = "v1" });

        c.AddSecurityDefinition ("Bearer", new ApiKeyScheme {
          Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = "header",
            Type = "apiKey"
        });

        c.AddSecurityRequirement (new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } } });

        c.OperationFilter<AcceptLanguageHeaderFilter> ();

        var app = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application;
        var xmlPath = System.IO.Path.Combine (app.ApplicationBasePath, "Tenli.Server.xml");

        c.IncludeXmlComments (xmlPath);
      });

      // services.AddLocalization(o =>
      // {
      //   o.ResourcesPath = "Resources";
      // });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IDistributedCache cache) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      } else {
        app.UseExceptionHandler ("/error");
      }

      app.UseCors (builder => builder
        .AllowAnyOrigin ()
        .AllowAnyMethod ()
        .AllowAnyHeader ()
        .AllowCredentials ());

      app.UseForwardedHeaders (new ForwardedHeadersOptions {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseAuthentication ();

      app.UseRequestLocalization ();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger ();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI (c => {
        c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Tenli API V1");
        c.RoutePrefix = "swagger";
      });

      app.UseMvc ();
    }
  }
}