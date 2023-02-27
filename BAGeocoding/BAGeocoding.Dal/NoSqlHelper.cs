using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAGeocoding.Utility;
using BAGeocoding.Entity.MapObj;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;

namespace BAGeocoding.Dal
{
    public class NoSqlHelper
    {
        //public static IMongoCollection<BAGSegmentV2> _collection;
        //public static MongoClient client = new MongoClient(Constants.MONGO_CONNECTION_STRING);
        //public static IMongoDatabase database = client.GetDatabase(Constants.MONGO_GEO_DB);
        public static MongoClient client = new MongoClient("");
        public static IMongoDatabase database = client.GetDatabase("");
        public static ConventionPack pack = new ConventionPack { new CamelCaseElementNameConvention() };
    }
}

        