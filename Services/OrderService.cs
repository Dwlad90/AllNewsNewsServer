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
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using SalesAudit.Data.Constants;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;
using Tenli.Server.Helpers;

namespace Tenli.Server.Services {
  public class OrderService {
    private ApplicationDbContext db;
    private CultureService _cultureService;
    private RedisService _redisService;
    private RedisKeysService _redisKeysService;
    private OrderStatusService _orderStatusService;

    public OrderService (
      ApplicationDbContext context,
      CultureService cultureService,
      RedisService redisService,
      RedisKeysService redisKeysService,
      OrderStatusService orderStatusService
    ) {
      db = context;
      _cultureService = cultureService;
      _redisService = redisService;
      _redisKeysService = redisKeysService;
      _orderStatusService = orderStatusService;
    }

    public async Task<List<Order>> GetOrdersAsync (int userId, string query, int offset, int limit, string culture = CultureKeys.English) {
      var queryable = db.Orders
        .Include (o => o.OrderStatus)
        .Where (x => x.IsActive &&
          (x.CustomerId == userId ||
            (x.ExecutorId == userId &&
              x.OrderStatus.Key != OrderStatusKeys.InDevelopment &&
              x.OrderStatus.Key != OrderStatusKeys.Available)
          )
        )
        .AsQueryable ();

      if (!string.IsNullOrEmpty (query)) {
        query = query.ToLower ();
        var words = query.Split ();

        foreach (var word in words) {
          queryable = queryable.Where (x => x.Name.ToLower ().Contains (word) || x.Description.ToLower ().Contains (word) || x.OrderProducts.Any (y => y.Product.Name.ToLower ().Contains (word) || y.Product.Description.ToLower ().Contains (word)));
        }
      }

      var orders = await queryable
        .OrderBy (x => x.Name)
        .Skip (offset).Take (limit)
        .ToListAsync ();

      if (orders.Count () == 0) {
        return orders;
      }

      int cultureId = _cultureService.GetCultureIdByKey (culture);
      return orders.Select (x => FillOrder (ref x, cultureId, culture)).ToList ();
    }

    public async Task<Order> GetOrderAsync (int id, int userId, string culture) {
      Order order = await GetOrderAsync (id, userId);

      if (order == null) {
        return null;
      }

      int cultureId = _cultureService.GetCultureIdByKey (culture);

      FillOrder (ref order, cultureId, culture);
      return order;
    }

    public async Task<Order> GetOrderAsync (int id, int userId) {
      Order order = await db.Orders
        .Include (o => o.OrderStatus)
        .FirstOrDefaultAsync (x => x.IsActive &&
          (x.CustomerId == userId ||
            (x.ExecutorId == userId &&
              x.OrderStatus.Key != OrderStatusKeys.InDevelopment &&
              x.OrderStatus.Key != OrderStatusKeys.Available)
          )
        );

      if (order == null) {
        return null;
      }

      return order;
    }

    public async Task<Order> GetOrderWithProductsAsync (int id, int userId) {
      Order order = await db.Orders
        .Include (o => o.OrderStatus)
        .Include (o => o.OrderProducts)
        .ThenInclude (o => o.Product)
        .FirstOrDefaultAsync (x => x.IsActive &&
          (x.CustomerId == userId ||
            (x.ExecutorId == userId &&
              x.OrderStatus.Key != OrderStatusKeys.InDevelopment &&
              x.OrderStatus.Key != OrderStatusKeys.Available)
          )
        );

      if (order == null) {
        return null;
      }

      return order;
    }

    private Order FillOrder (ref Order order, int cultureId, string culture) {

      db.Entry (order)
        .Collection (x => x.OrderProducts)
        .Query ()
        .Load ();

      foreach (var orderProduct in order.OrderProducts) {
        db.Entry (orderProduct)
          .Reference (x => x.Product)
          .Query ()
          .Load ();

        Product product = orderProduct.Product;

        db.Entry (product)
          .Reference (x => x.ProductType)
          .Query ()
          .Load ();

        string redisProductTypeKey = _redisKeysService.GetProductTypeKeyRedis (culture, product.ProductType);
        LocalizationResource productTypeResource = _redisService.Get<LocalizationResource> (redisProductTypeKey);

        if (product.ProductType.Names == null) {
          if (productTypeResource != null) {
            product.ProductType.Names = new List<LocalizationResource> { productTypeResource };
          } else {
            db.Entry (product.ProductType)
              .Collection (x => x.Names)
              .Query ()
              .Where (x => x.CultureId == cultureId)
              .Load ();

            _redisService.Set (redisProductTypeKey, product.ProductType.Names.FirstOrDefault ());
          }
        }

        db.Entry (product)
          .Reference (x => x.SizeUnit)
          .Query ()
          .Load ();

        if (product.SizeUnit != null && product.SizeUnit.Descriptions == null) {
          string redisSizeUnitKey = _redisKeysService.GetSizeUnitKeyRedis (culture, product.SizeUnit);
          LocalizationResource sizeUnitResource = _redisService.Get<LocalizationResource> (redisSizeUnitKey);

          if (sizeUnitResource != null) {
            product.SizeUnit.Descriptions = new List<LocalizationResource> { sizeUnitResource };
          } else {
            db.Entry (product.SizeUnit)
              .Collection (x => x.Descriptions)
              .Query ()
              .Where (x => x.CultureId == cultureId)
              .Load ();

            _redisService.Set (redisSizeUnitKey, product.SizeUnit.Descriptions.FirstOrDefault ());
          }
        }

        db.Entry (product)
          .Reference (x => x.WeightUnit)
          .Query ()
          .Load ();

        if (product.WeightUnit != null && product.WeightUnit.Descriptions == null) {
          string redisWeightUnitKey = _redisKeysService.GetWeithUnitKeyRedis (culture, product.WeightUnit);
          LocalizationResource weightUnitResource = _redisService.Get<LocalizationResource> (redisWeightUnitKey);

          if (weightUnitResource != null) {
            product.WeightUnit.Descriptions = new List<LocalizationResource> { weightUnitResource };
          } else {

            db.Entry (product.WeightUnit)
              .Collection (x => x.Descriptions)
              .Query ()
              .Where (x => x.CultureId == cultureId)
              .Load ();

            _redisService.Set (redisWeightUnitKey, product.WeightUnit.Descriptions.FirstOrDefault ());
          }
        }

        if (product.VolumeUnit != null && product.VolumeUnit.Descriptions == null) {
          string redisVolumeUnitKey = _redisKeysService.GetVolumeUnitKeyRedis (culture, product.VolumeUnit);
          LocalizationResource volumeUnitResource = _redisService.Get<LocalizationResource> (redisVolumeUnitKey);

          if (volumeUnitResource != null) {
            product.VolumeUnit.Descriptions = new List<LocalizationResource> { volumeUnitResource };
          } else {

            db.Entry (product.VolumeUnit)
              .Collection (x => x.Descriptions)
              .Query ()
              .Where (x => x.CultureId == cultureId)
              .Load ();

            _redisService.Set (redisVolumeUnitKey, product.VolumeUnit.Descriptions.FirstOrDefault ());
          }
        }

        db.Entry (product)
          .Reference (x => x.Currency)
          .Query ()
          .Load ();

        if (product.Currency.Names == null) {
          string redisCurrencyKey = _redisKeysService.GetCurrencyKeyRedis (culture, product.Currency);
          LocalizationResource currencyResource = _redisService.Get<LocalizationResource> (redisCurrencyKey);

          if (currencyResource != null) {
            product.Currency.Names = new List<LocalizationResource> { currencyResource };
          } else {

            db.Entry (product.Currency)
              .Collection (x => x.Names)
              .Query ()
              .Where (x => x.CultureId == cultureId)
              .Load ();

            _redisService.Set (redisCurrencyKey, product.Currency.Names.FirstOrDefault ());
          }
        }
      }

      db.Entry (order)
        .Reference (x => x.DeliveryType)
        .Query ()
        .Load ();

      if (order.DeliveryType.Names == null) {
        string redisDeliveryTypeKey = _redisKeysService.GetDeliveryTypeKeyRedis (culture, order.DeliveryType);
        LocalizationResource deliveryTypeResource = _redisService.Get<LocalizationResource> (redisDeliveryTypeKey);

        if (deliveryTypeResource != null) {
          order.DeliveryType.Names = new List<LocalizationResource> { deliveryTypeResource };
        } else {

          db.Entry (order.DeliveryType)
            .Collection (x => x.Names)
            .Query ()
            .Where (x => x.CultureId == cultureId)
            .Load ();

          _redisService.Set (redisDeliveryTypeKey, order.DeliveryType.Names.FirstOrDefault ());
        }
      }

      db.Entry (order)
        .Reference (x => x.OrderStatus)
        .Query ()
        .Load ();

      if (order.OrderStatus.Names == null) {
        string redisOrderStatusKey = _redisKeysService.GetOrderStatusKeyRedis (culture, order.OrderStatus);
        LocalizationResource orderStatusResource = _redisService.Get<LocalizationResource> (redisOrderStatusKey);

        if (orderStatusResource != null) {
          order.OrderStatus.Names = new List<LocalizationResource> { orderStatusResource };
        } else {

          db.Entry (order.OrderStatus)
            .Collection (x => x.Names)
            .Query ()
            .Where (x => x.CultureId == cultureId)
            .Load ();

          _redisService.Set (redisOrderStatusKey, order.OrderStatus.Names.FirstOrDefault ());
        }
      }

      return order;
    }

    public Task UpdateOrderAsync (Order order) {
      db.Entry<Order> (order).State = EntityState.Modified;
      return db.SaveChangesAsync ();
    }

    public Task AddOrderAsync (Order order) {
      db.Orders.Add (order);
      return db.SaveChangesAsync ();
    }

    public Task<List<Product>> GetOrderProductsAsync (int orderId, int userId, string query, int offset, int limit, string culture = CultureKeys.English) {
      var queryable = db.Products
        .Include (o => o.OrderProducts)
        .ThenInclude (o => o.Order)
        .ThenInclude (o => o.OrderStatus)
        .Where (x =>
          x.OrderProducts.Any (y => y.OrderId == orderId && x.IsActive == true && (y.Order.CustomerId == userId || (y.Order.ExecutorId == userId &&
            y.Order.OrderStatus.Key != OrderStatusKeys.InDevelopment &&
            y.Order.OrderStatus.Key != OrderStatusKeys.Available))))
        .AsQueryable ();

      if (!string.IsNullOrEmpty (query)) {
        query = query.ToLower ();
        var words = query.Split ();

        foreach (var word in words) {
          queryable = queryable
            .Where (x => x.Name.ToLower ().Contains (word) ||
              x.Description.ToLower ().Contains (word) ||
              x.Id.ToString () == word);
        }
      }

      return queryable
        .OrderByDescending (x => x.CreationDateTime)
        .ThenBy (x => x.Id)
        .Skip (offset).Take (limit)
        .ToListAsync ();
    }

    public Task<List<Product>> GetOrderProductsAsync (int orderId, int userId) {
      return db.Products
        .Include (o => o.OrderProducts)
        .ThenInclude (o => o.Order)
        .Where (x => x.OrderProducts.Any (y => y.OrderId == orderId && x.IsActive == true && (y.Order.ExecutorId == userId || y.Order.CustomerId == userId))).ToListAsync ();
    }

    public async Task<List<Order>> GetOrderAroundPositionAsync (Point userPosition, double radius, string query, int offset, int limit, int cultureId, string culture) {
      int orderStatusAvailableId = _orderStatusService.GetOrderStatusIdByKey (OrderStatusKeys.Available);

      var queryable = db.Orders
        .Include (o => o.OrderProducts)
        .ThenInclude (o => o.Product)
        .Where (x => x.IsActive == true && x.ExecutorId == null && x.OrderStatusId == orderStatusAvailableId && LocationHelpers.ComparePosition (x.StartLocation, userPosition, radius)).AsQueryable ();

      if (!string.IsNullOrEmpty (query)) {
        query = query.ToLower ();
        var words = query.Split ();

        foreach (var word in words) {
          queryable = queryable.Where (x => x.Name.ToLower ().Contains (word) || x.Description.ToLower ().Contains (word) || x.OrderProducts.Any (y => y.Product.Name.ToLower ().Contains (word) || y.Product.Description.ToLower ().Contains (word)));
        }
      }

      var orders = await queryable
        .OrderBy (x => x.Name)
        .Skip (offset).Take (limit)
        .ToListAsync ();

      if (orders.Count () == 0) {
        return orders;
      }

      return orders.Select (x => FillOrder (ref x, cultureId, culture)).ToList ();
    }

    public bool OrderCanBeModified (Order order) {
      return order.OrderStatus.Key == OrderStatusKeys.InDevelopment || order.OrderStatus.Key == OrderStatusKeys.Available;
    }
  }
}