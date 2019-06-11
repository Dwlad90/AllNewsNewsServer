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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;

namespace Tenli.Server.Services {
  public class ProductService {
    private ApplicationDbContext db;
    private CultureService _cultureService;
    private RedisService _redisService;
    private RedisKeysService _redisKeysService;

    public ProductService (ApplicationDbContext context, CultureService cultureService, RedisService redisService, RedisKeysService redisKeysService) {
      db = context;
      _cultureService = cultureService;
      _redisService = redisService;
      _redisKeysService = redisKeysService;
    }

    public Task UpdateProductAsync (Product product) {
      db.Entry<Product> (product).State = EntityState.Modified;
      return db.SaveChangesAsync ();
    }

    public Task AddProductAsync (Product product) {
      db.Products.Add (product);
      return db.SaveChangesAsync ();
    }

    public Task UpdateMultipleProductsAsync (List<Product> products) {
      products.ForEach (product => {
        db.Entry<Product> (product).State = EntityState.Modified;
      });

      return db.SaveChangesAsync ();
    }
    public Task AddMultipleProductsAsync (List<Product> products) {
      db.Products.AddRange (products);
      return db.SaveChangesAsync ();
    }
  }
}