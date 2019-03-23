using System;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Function
{
    public static class MongoDatabase
    {
        /// <summary>
        /// A reference to an IMongoDatabase.
        /// </summary>
        private static IMongoDatabase Database;

        /// <summary>
        /// A reference to an IMongoCollection<BsonDocument>.
        /// </summary>
        private static IMongoCollection<BsonDocument> Collection;

        /// <summary>
        /// Sets up the MongoDB connection.
        /// </summary>
        public static void SetupConnection(this DatabaseFunctionHandler databaseFunctionHandler)
        {
            if (Database == null)
            {
                var databaseName = Environment.GetEnvironmentVariable("mongo_database_name") ?? "default_database";
                var databaseUser = Environment.GetEnvironmentVariable("mongo_database_user") ?? "admin";
                var databasePassword = GetPassword();
				
                var mongoCredential = MongoCredential.CreateCredential(databaseName, databaseUser, databasePassword);
                var mongoClientSettings = new MongoClientSettings
                {
                    Credential =  mongoCredential,
                    Server = new MongoServerAddress(Environment.GetEnvironmentVariable("mongo_endpoint"), 27017)
                };

                var client = new MongoClient(mongoClientSettings);
                Database = client.GetDatabase(databaseName);

                var collectionName = Environment.GetEnvironmentVariable("mongo_collection_name") ?? "default_collection";
                Collection = Database.GetCollection<BsonDocument>(collectionName);
            }
        }

        /// <summary>
        /// Gets the Mongo database of this instance.
        /// </summary>
        /// <param name="functionHandler">This instance of a function handler.</param>
        /// <returns>An IMongoDatabase.</returns>
        public static IMongoDatabase GetDatabase(this DatabaseFunctionHandler functionHandler)
        {
            return Database;
        }
        
        /// <summary>
        /// Gets the Mongo collection of this instance.
        /// </summary>
        /// <param name="functionHandler">This instance of a function handler.</param>
        /// <returns>An IMongoCollection<BsonDocument>.</returns>
        public static IMongoCollection<BsonDocument> GetCollection(this DatabaseFunctionHandler functionHandler)
        {
            return Collection;
        }

        private static string GetPassword()
        {
            return Environment.GetEnvironmentVariable("mongo_database_password") 
				?? File.ReadAllText("/var/openfaas/secrets/mongodb-auth").Trim();
        }

    }
}