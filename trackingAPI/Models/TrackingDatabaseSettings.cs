namespace trackingAPI.Models
{
    public class TrackingDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CustomerCollectionName { get; set; } = null!;
    }
}
