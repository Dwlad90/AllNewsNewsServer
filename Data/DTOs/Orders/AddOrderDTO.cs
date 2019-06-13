using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using AllNewsServer.Data.DTOs.Product;

namespace AllNewsServer.Data.DTOs.Order {
  public class AddOrderDTO {

    [Required]
    public string Name { get; set; }
    public string Description { get; set; }

    [Required]
    public List<AddProductDTO> Products { get; set; }

    [Required]
    public DateTime ExpectingDateTime { get; set; }

    [Required]
    [JsonConverter (typeof (GeometryConverter))]
    public Point StartLocation { get; set; }

    [Required]
    [JsonConverter (typeof (GeometryConverter))]
    public Point EndLocation { get; set; }
    public int DeliveryTypeId { get; set; }
  }
}