using MongoDB.Driver;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
        _database = client.GetDatabase("BlackFarmDB");
    }

    public IMongoCollection<T> GetCollection<T>(string name){
        return _database.GetCollection<T>(name);
    }
}