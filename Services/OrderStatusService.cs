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
  public class OrderStatusService {
    private ApplicationDbContext db;
    private CultureService _cultureService;
    private RedisService _redisService;
    private RedisKeysService _redisKeysService;

    public OrderStatusService (ApplicationDbContext context, RedisService redisService, CultureService cultureService, RedisKeysService redisKeysService) {
      db = context;
      _redisService = redisService;
      _cultureService = cultureService;
      _redisKeysService = redisKeysService;
    }

    public async Task<List<OrderStatus>> GetOrderStatusesAsync (string culture = CultureKeys.English) {
      int cultureId = _cultureService.GetCultureIdByKey (culture);
      var orderStatuses = await db.OrderStatuses.ToListAsync ();

      foreach (var orderStatus in orderStatuses) {

        string redisKey = _redisKeysService.GetOrderStatusKeyRedis (culture, orderStatus);
        LocalizationResource localizationResource = _redisService.Get<LocalizationResource> (redisKey);

        if (localizationResource != null) {
          orderStatus.Names = new List<LocalizationResource> { localizationResource };
        } else {
          db.Entry (orderStatus)
            .Collection (x => x.Names)
            .Query ()
            .Where (x => x.CultureId == cultureId)
            .Load ();

          _redisService.Set (redisKey, orderStatus.Names.FirstOrDefault ());
        }
      }

      return orderStatuses;
    }

    public async Task<OrderStatus> GetOrderStatusByKeyAsync (string key, int cultureId) {
      var orderStatus = await db.OrderStatuses.SingleOrDefaultAsync (x => x.Key == key);

      db.Entry (orderStatus)
        .Collection (x => x.Names)
        .Query ()
        .Where (x => x.CultureId == cultureId)
        .Load ();

      return orderStatus;
    }

    public int GetOrderStatusIdByKey (string key) {
      return db.OrderStatuses
        .Single (x => x.Key == key).Id;
    }
  }
}