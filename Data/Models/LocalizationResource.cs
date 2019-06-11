using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Tenli.Server.Data.Models {
  public class LocalizationResource : BaseEntity {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public int CultureId { get; set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual Culture Culture { get; set; }
    public int? OrderStatusId { get; set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual OrderStatus OrederStatus { get; set; }
    public int? ProductTypeId { get; set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ProductType ProductType { get; set; }
    public int? DeliveryTypeId { get; set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual DeliveryType DeliveryType { get; set; }
  }
}