using Microsoft.Extensions.Options;
using MongoDB.Driver;
using trackingAPI.Models;

namespace trackingAPI.Database
{
    public class DBContext
    {
        public IMongoCollection<tbCustomer> customersCollection;
        public DBContext(
        IOptions<TrackingDatabaseSettings> trackingDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                trackingDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                trackingDatabaseSettings.Value.DatabaseName);

            customersCollection = mongoDatabase.GetCollection<tbCustomer>(
                trackingDatabaseSettings.Value.CustomerCollectionName);
        }

       
    }
}
