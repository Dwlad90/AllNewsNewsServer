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
  public class RedisKeysService {
    public string GetOrderStatusKeyRedis (string culture, OrderStatus orderStatus) {
      return RedisKeys.OrderStatus + culture + ":" + orderStatus.Key;
    }
    public string GetDeliveryTypeKeyRedis (string culture, DeliveryType deliveryType) {
      return RedisKeys.DeliveryType + culture + ":" + deliveryType.Key;
    }
    public string GetProductTypeKeyRedis (string culture, ProductType productType) {
      return RedisKeys.ProductType + culture + ":" + productType.Key;
    }
    public string GetCurrencyKeyRedis (string culture, Currency currency) {
      return RedisKeys.Currency + culture + ":" + currency.Key;
    }
    public string GetWeithUnitKeyRedis (string culture, WeightUnit weightUnit) {
      return RedisKeys.WeightUnit + culture + ":" + weightUnit.Key;
    }
    public string GetSizeUnitKeyRedis (string culture, SizeUnit sizeUnit) {
      return RedisKeys.SizeUnit + culture + ":" + sizeUnit.Key;
    }
      public string GetVolumeUnitKeyRedis (string culture, VolumeUnit volumeUnit) {
      return RedisKeys.VolumeUnit + culture + ":" + volumeUnit.Key;
    }
  }
}