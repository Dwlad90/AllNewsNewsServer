using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AllNewsServer.Data.Models {
  public class News {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Site { get; set; }
    public string Img { get; set; }
    public double Date { get; set; }
    public List<string> Text { get; set; }
    public Boolean IsActive { get; set; } = true;
    public Boolean IsPinned { get; set; } = false;
    public Boolean IsOpened { get; set; } = false;
    public Boolean IsFavorited { get; set; } = false;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int __v { get; set; }
  }
}