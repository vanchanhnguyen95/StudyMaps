using ElasticProject.Data.Entity;
using ElasticProject.Data.Entity.MapObj;
using Nest;

namespace ElasticProject.Data.Mapping
{
    public static class Mapping
    {
        public static CreateIndexDescriptor CitiesMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<Cities>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.Id))
                .Text(t => t.Name(n => n.City))
                .Text(t => t.Name(n => n.Region))
                .Number(t => t.Name(n => n.Population))
                .Date(t => t.Name(n => n.CreateDate)))
            );
        }

        public static CreateIndexDescriptor HealthcareMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<HealthCareModel>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.id))
                .Text(t => t.Name(n => n.name))
                //.Text(t => t.Name(n => n.address))
                .Text(t => t.Name(n => n.keywords))
                .Text(t => t.Name(n => n.specialist))
                .Number(t => t.Name(n => n.Latitude))
                .Number(t => t.Name(n => n.Longitude))
                .GeoPoint(t => t.Name(n => n.geoLocation))
                //.GeoShape(t => t.Name(n ))
                )
                
            );
        }

        public static CreateIndexDescriptor ElasticRequestCreateGeopointMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<ElasticRequestCreateGeopoint>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.typeid))
                .Number(t => t.Name(n => n.indexid))
                .Number(t => t.Name(n => n.shapeid))
                .Text(t => t.Name(n => n.kindname))
                .Text(t => t.Name(n => n.name))
                .Text(t => t.Name(n => n.address))
                .Text(t => t.Name(n => n.shortkey))
                .GeoPoint(t => t.Name(n => n.location))
                )
            );
        }

        public static CreateIndexDescriptor ElasticRequestCreateGeoshapeMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<ElasticRequestCreateGeopoint>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.typeid))
                .Number(t => t.Name(n => n.indexid))
                .Number(t => t.Name(n => n.shapeid))
                .Text(t => t.Name(n => n.kindname))
                .Text(t => t.Name(n => n.name))
                .Text(t => t.Name(n => n.address))
                .Text(t => t.Name(n => n.shortkey))
                .GeoShape(t => t.Name(n => n.location ))
                )
            );
        }

    }
}
