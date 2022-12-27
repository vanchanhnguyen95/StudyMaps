using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace AspMongoApi.Models
{
    public class Neighborhood
    {
        public ObjectId Id { get; set; }
        public GeoJsonPoint<GeoJson2DCoordinates> Geometry { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}