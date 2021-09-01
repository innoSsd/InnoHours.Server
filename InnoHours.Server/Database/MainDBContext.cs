using MongoDB.Driver;

namespace InnoHours.Server.DataBase
{
    public class MainDbContext
    {
        internal MongoClient Client { get; } = new("mongodb+srv://admin:1234@cluster0.lpppq.mongodb.net/main?retryWrites=true&w=majority");
        internal IMongoDatabase MainDatabase { get; set; }

        public MainDbContext()
        {
            MainDatabase = Client.GetDatabase("main");
        }
    }
}