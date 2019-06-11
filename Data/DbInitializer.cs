using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesAudit.Data.Constants;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.Models;
using Tenli.Server.Helpers;
using Tenli.Server.Services;

namespace Tenli.Server.Data {
  public class DbInitializer {
    public static void Seed (ApplicationDbContext context, RedisService redisSevice) {
      Console.WriteLine ("Performing database initializaton...");
      Stopwatch stopWatch = Stopwatch.StartNew ();

      try {
        context.Database.EnsureCreated ();

        AddOrUpdateUsers (context);
        AddOrUpdateRoles (context);
        AddOrUpdateCultures (context, redisSevice);

        context.SaveChanges ();

        AddOrUpdateLocalizationResources (context, redisSevice);

        context.SaveChanges ();

        AddOrUpdateOrderStatuses (context, redisSevice);
        AddOrUpdateProductTypes (context, redisSevice);
        AddOrUpdateDeliveryTypes (context, redisSevice);
        AddOrUpdateWeightUnits (context, redisSevice);
        AddOrUpdateVolumeUnits (context, redisSevice);
        AddOrUpdateSizeUnits (context, redisSevice);
        AddOrUpdateCurrencyUnits (context, redisSevice);

        context.SaveChanges ();

        ApplyAdminRoles (context);

        context.SaveChanges ();
      } catch (Exception e) {
        Console.WriteLine ("Failed to initialize database. Ignore this if you are running 'migrations add' command");
        System.Console.WriteLine (e);
      }

      Console.WriteLine ("Database initialized: elapsed time {0} ms", stopWatch.ElapsedMilliseconds);
    }

    private static void AddOrUpdateUsers (ApplicationDbContext context) {
      var users = new List<ApplicationUser> () {
        new ApplicationUser { Phone = "972532403308", Email = "dwlad90@gmail.com", Password = "password" }
      };

      foreach (var user in users) {
        var exists = context.ApplicationUsers.Any (x => x.Phone == user.Phone);

        if (!exists) {
          context.ApplicationUsers.Add (user);
        }
      }
    }

    private static void AddOrUpdateRoles (ApplicationDbContext context) {
      var roles = new List<ApplicationRole> () {
        new ApplicationRole { Name = ApplicationRoleNames.Admin, Description = "Администратор системы" },
        new ApplicationRole { Name = ApplicationRoleNames.TopManager, Description = "Менеджер, который имеет доступ ко всем проектам" },
        new ApplicationRole { Name = ApplicationRoleNames.Manager, Description = "Менеджер, который имеет доступ только к своим проектам" },
        new ApplicationRole { Name = ApplicationRoleNames.Operator, Description = "Оператор, проверяющий задания, выполненные пользователями мобильного приложения" },
      };

      foreach (var role in roles) {
        var exists = context.ApplicationRoles.Any (x => x.Name == role.Name);

        if (!exists) {
          context.ApplicationRoles.Add (role);
        }
      }
    }

    private static void ApplyAdminRoles (ApplicationDbContext context) {
      var administrator = context.ApplicationUsers
        .Include (x => x.UserRoles)
        .ThenInclude (x => x.ApplicationRole)
        .SingleOrDefault (x => x.Phone == "972532403308");

      if (administrator != null) {
        var isAdmin = administrator.UserRoles.Any (x => x.ApplicationRole.Name == "Admin");

        if (!isAdmin) {
          var adminRole = context.ApplicationRoles.SingleOrDefault (x => x.Name == "Admin");

          if (adminRole != null)
            administrator.UserRoles.Add (new UserRole { ApplicationRoleId = adminRole.Id });
        }
      }
    }
    private static void AddOrUpdateCultures (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (CultureKeys)).ForEach ((type) => {
        Culture culture = new Culture { Key = type.Value, Description = type.Key };

        var item = context.Cultures.FirstOrDefault (x => x.Key == type.Value);

        if (item == null) {
          context.Cultures.Add (culture);
          item = culture;
        }

        redisService.Set (RedisKeys.Culture + item.Key, item);
      });
    }
    private static void AddOrUpdateLocalizationResources (ApplicationDbContext context, RedisService redisService) {
      #region OrderStatus resources
      var orderStatusLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = OrderStatusKeys.InDevelopment, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "In development", },
        new LocalizationResource { Key = OrderStatusKeys.InDevelopment, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "בפיתוח" },
        new LocalizationResource { Key = OrderStatusKeys.Available, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Available", },
        new LocalizationResource { Key = OrderStatusKeys.Available, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "זמין" },
        new LocalizationResource { Key = OrderStatusKeys.Accepted, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Accepted" },
        new LocalizationResource { Key = OrderStatusKeys.Accepted, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "התקבל" },
        new LocalizationResource { Key = OrderStatusKeys.InProgress, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "In progress" },
        new LocalizationResource { Key = OrderStatusKeys.InProgress, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "בתהליך" },
        new LocalizationResource { Key = OrderStatusKeys.Handing, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Handing" },
        new LocalizationResource { Key = OrderStatusKeys.Handing, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מסירה" },
        new LocalizationResource { Key = OrderStatusKeys.Done, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Done" },
        new LocalizationResource { Key = OrderStatusKeys.Done, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "בוצע" },
        new LocalizationResource { Key = OrderStatusKeys.Rejected, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Rejected" },
        new LocalizationResource { Key = OrderStatusKeys.Rejected, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "נדחה" },
        new LocalizationResource { Key = OrderStatusKeys.Cancelled, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Cancelled" },
        new LocalizationResource { Key = OrderStatusKeys.Cancelled, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מבוטל" }
      };
      #endregion OrderStatus resources

      #region ProductType resources
      var productTypeLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = ProductTypeKeys.Box, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Box" },
        new LocalizationResource { Key = ProductTypeKeys.Box, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "תיבה" },
        new LocalizationResource { Key = ProductTypeKeys.Bag, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Bag" },
        new LocalizationResource { Key = ProductTypeKeys.Bag, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "חבילה" },
        new LocalizationResource { Key = ProductTypeKeys.Envelop, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Envelop" },
        new LocalizationResource { Key = ProductTypeKeys.Envelop, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מעטפה" },
        new LocalizationResource { Key = ProductTypeKeys.Thing, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Thing" },
        new LocalizationResource { Key = ProductTypeKeys.Thing, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "דבר" },
      };
      #endregion ProductType resource

      #region WeightUnit resources
      var weithtUnitLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = WeightUnitKeys.Kilogram, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "kg" },
        new LocalizationResource { Key = WeightUnitKeys.Kilogram, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "קילוגרם" },
        new LocalizationResource { Key = WeightUnitKeys.Gram, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "g" },
        new LocalizationResource { Key = WeightUnitKeys.Gram, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "גרם" },
        new LocalizationResource { Key = WeightUnitKeys.Pound, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "lb" },
        new LocalizationResource { Key = WeightUnitKeys.Pound, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "ליברה" },
      };
      #endregion WeightUnit resources

      #region Volume resources
      var volumeUnitLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = VolumeUnitKeys.Liter, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "l" },
        new LocalizationResource { Key = VolumeUnitKeys.Liter, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "ליטר" },
        new LocalizationResource { Key = VolumeUnitKeys.CubicMetere, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "m3" },
        new LocalizationResource { Key = VolumeUnitKeys.CubicMetere, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מטר מעוקב" },
        new LocalizationResource { Key = VolumeUnitKeys.CubicCentimetre, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "cm3" },
        new LocalizationResource { Key = VolumeUnitKeys.CubicCentimetre, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "ליברהסנטימטר מעוקב" },
      };
      #endregion VolumeUnit resource

      #region Size resources
      var sizeUnitLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = SizeUnitKeys.Centimetre, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "cm" },
        new LocalizationResource { Key = SizeUnitKeys.Centimetre, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "סנטימטר" },
        new LocalizationResource { Key = SizeUnitKeys.Metere, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "m" },
        new LocalizationResource { Key = SizeUnitKeys.Metere, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מטר" },
        new LocalizationResource { Key = SizeUnitKeys.Foot, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "ft" },
        new LocalizationResource { Key = SizeUnitKeys.Foot, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "רגל" },
        new LocalizationResource { Key = SizeUnitKeys.Inch, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "inch" },
        new LocalizationResource { Key = SizeUnitKeys.Inch, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "'אינץ" },
      };
      #endregion SizeUnit resources

      #region Currency resources
      var currencyLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = CurrencyKeys.USDollar, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "United States dollar" },
        new LocalizationResource { Key = CurrencyKeys.USDollar, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "דולר אמריקני" },
        new LocalizationResource { Key = CurrencyKeys.PoundSterling, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Pound sterling" },
        new LocalizationResource { Key = CurrencyKeys.PoundSterling, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "לירה שטרלינג" },
        new LocalizationResource { Key = CurrencyKeys.Shekel, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Israeli new shekel" },
        new LocalizationResource { Key = CurrencyKeys.Shekel, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "שקל חדש" },
        new LocalizationResource { Key = CurrencyKeys.RussianRuble, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Russian ruble" },
        new LocalizationResource { Key = CurrencyKeys.RussianRuble, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "רובל רוסי" },
      };
      #endregion Currency resources

      #region DeliveryType resources
      var deliveryTypeLocalizationResources = new List<LocalizationResource> () {
        new LocalizationResource { Key = DeliveriTypeKeys.Car, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Car" },
        new LocalizationResource { Key = DeliveriTypeKeys.Car, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מְכוֹנִית" },
        new LocalizationResource { Key = DeliveriTypeKeys.Train, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Train" },
        new LocalizationResource { Key = DeliveriTypeKeys.Train, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "רכבת" },
        new LocalizationResource { Key = DeliveriTypeKeys.Plane, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Plane" },
        new LocalizationResource { Key = DeliveriTypeKeys.Plane, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "מטוס" },
        new LocalizationResource { Key = DeliveriTypeKeys.Ship, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.English), Value = "Ship" },
        new LocalizationResource { Key = DeliveriTypeKeys.Ship, Culture = context.Cultures.FirstOrDefault (x => x.Key == CultureKeys.Hebrew), Value = "אניה" },
      };
      #endregion DeliveryType resource

      saveLocalizationResource (context, redisService, RedisKeys.OrderStatus, orderStatusLocalizationResources);
      saveLocalizationResource (context, redisService, RedisKeys.ProductType, productTypeLocalizationResources);
      saveLocalizationResource (context, redisService, RedisKeys.WeightUnit, weithtUnitLocalizationResources);
      saveLocalizationResource (context, redisService, RedisKeys.VolumeUnit, volumeUnitLocalizationResources);
      saveLocalizationResource (context, redisService, RedisKeys.SizeUnit, sizeUnitLocalizationResources);
      saveLocalizationResource (context, redisService, RedisKeys.Currency, currencyLocalizationResources);
      saveLocalizationResource (context, redisService, RedisKeys.DeliveryType, deliveryTypeLocalizationResources);
    }

    private static void saveLocalizationResource (ApplicationDbContext context, RedisService redisService, string redisKey, List<LocalizationResource> orderStatusLocalizationResources) {
      foreach (var localizationResource in orderStatusLocalizationResources) {
        var exists = context.LocalizationResources.Any (x => x.Key == localizationResource.Key && x.Culture.Key == localizationResource.Culture.Key);

        LocalizationResource item;
        if (!exists) {
          context.LocalizationResources.Add (localizationResource);
          item = localizationResource;
        } else {
          item = context.LocalizationResources.Include (x => x.Culture).FirstOrDefault (x => x.Key == localizationResource.Key && x.Culture.Key == localizationResource.Culture.Key);
        }

        redisService.Set (redisKey + item.Culture.Key + ":" + item.Key, item);
      }
    }

    private static void AddOrUpdateOrderStatuses (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (OrderStatusKeys)).ForEach (type => {
        OrderStatus orderStatus = new OrderStatus {
        Key = type.Value, Names = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.OrderStatuses.FirstOrDefault (x => x.Key == orderStatus.Key);

        if (item == null) {
          context.OrderStatuses.Add (orderStatus);
        } else {
          item.Names = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }

    private static void AddOrUpdateProductTypes (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (ProductTypeKeys)).ForEach (type => {
        ProductType productType = new ProductType {
        Key = type.Value, Names = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.ProductTypes.FirstOrDefault (x => x.Key == productType.Key);

        if (item == null) {
          context.ProductTypes.Add (productType);
        } else {
          item.Names = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }

    private static void AddOrUpdateWeightUnits (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (WeightUnitKeys)).ForEach (type => {
        WeightUnit weightUnit = new WeightUnit {
        Key = type.Value, Descriptions = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.WeightUnits.FirstOrDefault (x => x.Key == weightUnit.Key);

        if (item == null) {
          context.WeightUnits.Add (weightUnit);
        } else {
          item.Descriptions = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }

    private static void AddOrUpdateVolumeUnits (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (VolumeUnitKeys)).ForEach (type => {
        VolumeUnit volumeUnit = new VolumeUnit {
        Key = type.Value, Descriptions = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.VolumeUnits.FirstOrDefault (x => x.Key == volumeUnit.Key);

        if (item == null) {
          context.VolumeUnits.Add (volumeUnit);
        } else {
          item.Descriptions = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }

    private static void AddOrUpdateSizeUnits (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (SizeUnitKeys)).ForEach (type => {
        SizeUnit sizeUnit = new SizeUnit {
        Key = type.Value, Descriptions = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.SizeUnits.FirstOrDefault (x => x.Key == sizeUnit.Key);

        if (item == null) {
          context.SizeUnits.Add (sizeUnit);
        } else {
          item.Descriptions = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }

    private static void AddOrUpdateCurrencyUnits (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (CurrencyKeys)).ForEach (type => {
        Currency currency = new Currency {
        Key = type.Value, Names = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.Currencies.FirstOrDefault (x => x.Key == currency.Key);

        if (item == null) {
          context.Currencies.Add (currency);
        } else {
          item.Names = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }

    private static void AddOrUpdateDeliveryTypes (ApplicationDbContext context, RedisService redisService) {
      TypeHelpers.getTypeValues (typeof (DeliveriTypeKeys)).ForEach (type => {
        DeliveryType deliveryType = new DeliveryType {
        Key = type.Value, Names = context.LocalizationResources.Where (x => x.Key == type.Value).ToList ()
        };

        var item = context.DeliveryTypes.FirstOrDefault (x => x.Key == deliveryType.Key);

        if (item == null) {
          context.DeliveryTypes.Add (deliveryType);
        } else {
          item.Names = context.LocalizationResources.Where (x => x.Key == item.Key).ToList ();
        }
      });
    }
  }
}