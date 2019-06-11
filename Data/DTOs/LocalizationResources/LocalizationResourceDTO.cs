using System;
using System.Collections.Generic;
using Tenli.Server.Data.DTOs.Culture;
using Tenli.Server.Data.DTOs.OrderStatus;
using Tenli.Server.Data.DTOs.ProductType;

namespace Tenli.Server.Data.DTOs.LocalizationResource {
  public class LocalizationResourceDTO {
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public virtual CultureDTO Culture { get; set; }
    public virtual OrderStatusDTO OrederStatus { get; set; }
    public int? ProductTypeId { get; set; }
    public virtual ProductTypeDTO ProductType { get; set; }
  }
}