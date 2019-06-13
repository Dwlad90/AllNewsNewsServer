using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AllNewsServer.Data;
using AllNewsServer.Data.Constants;
using AllNewsServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace AllNewsServer.Services {
  public class NewsService {
    private readonly IMongoCollection<News> _news;

    public NewsService (MongoSettings settings) {

      var client = new MongoClient (settings.ConnectionString);
      var database = client.GetDatabase (settings.Database);

      _news = database.GetCollection<News> ("news");
    }

    public List<News> GetNews (int skip) {
      var news = _news.Find (x => x.IsActive).SortByDescending (x => x.Date).Skip (skip).Limit (20).ToList ();

      return news;
    }
  }
}