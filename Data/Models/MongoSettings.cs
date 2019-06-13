namespace AllNewsServer.Data.Models {
  public class MongoSettings : IMongoSettings {
    public string ConnectionString { get; set; }
    public string Database { get; set; }
  }

  public interface IMongoSettings {
    string ConnectionString { get; set; }
    string Database { get; set; }
  }
}