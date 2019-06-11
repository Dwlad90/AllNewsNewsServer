using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;

namespace Tenli.Server.Services {
  public class CultureService {
    private ApplicationDbContext db;

    public CultureService (ApplicationDbContext context) {
      db = context;
    }

    public IRequestCultureFeature GetCultureFromRequest (HttpRequest request) {
      return request.HttpContext.Features.Get<IRequestCultureFeature> ();
    }

    public Task<List<Culture>> GetCulturesAsync () {
      return db.Cultures
        .ToListAsync ();
    }

    public int GetCultureIdByKey (string key) {
      return db.Cultures
        .Single (x => x.Key == key).Id;
    }

    public int GetCultureIdForRequest(HttpRequest request) {
      var cultures = GetCultureFromRequest (request);

      return db.Cultures
        .Single (x => x.Key == cultures.RequestCulture.Culture.Name).Id;
    }
  }
}