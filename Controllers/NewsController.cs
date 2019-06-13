using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllNewsServer.Data.Models;
using AllNewsServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AllNewsServer.Controllers {
  // [Authorize]
  [Route ("api/[controller]")]
  [ApiController]
  public class NewsController : ControllerBase {
    NewsService _newsService;

    public NewsController (NewsService newsService) {
      _newsService = newsService;
    }

    // GET api/news
    [HttpGet]
    public ActionResult<List<News>> GetList ([FromQuery] int skip) {
      // return new string[] { "value1", "value2" };
      List<News> news = _newsService.GetNews (skip);
      return news;
    }

    // GET api/values/5
    [HttpGet ("{id}")]
    public ActionResult<string> Get (int id) {
      return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post ( /* [FromBody] string value*/ ) {
      Ok ();
    }

    // PUT api/values/5
    [HttpPut ("{id}")] public void Put (int id, [FromBody] string value) { }

    // DELETE api/values/5
    [HttpDelete ("{id}")] public void Delete (int id) { }
  }
}