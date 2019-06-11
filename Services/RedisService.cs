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
using StackExchange.Redis;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;

namespace Tenli.Server.Services {
  public class RedisService {
    private ApplicationDbContext db;
    private ConnectionMultiplexer redis;
    private IDatabase redisDb;

    public RedisService (ApplicationDbContext context, IConfiguration configuration) {
      db = context;
      redis = ConnectionMultiplexer.Connect (configuration.GetConnectionString ("Redis"));
      redisDb = redis.GetDatabase ();
    }
    public Task<bool> SetAsync<T> (string key, T value) {
      string serializedValue = value is string ? value.ToString () : JsonConvert.SerializeObject (value);
      return redisDb.StringSetAsync (key, serializedValue);
    }

    public bool Set<T> (string key, T value) {
      string serializedValue = value is string ? value.ToString () : JsonConvert.SerializeObject (value);
      return redisDb.StringSet (key, serializedValue);
    }

    public T Get<T> (string key) {
      if (!Exist (key)) {
        return default (T);
      }
      return JsonConvert.DeserializeObject<T> (redisDb.StringGet (key));

    }

    public bool Exist (string key) {
      return redisDb.KeyExists (key);
    }
  }
}