using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Mapping.Test
{
    public static partial class Mapping
    {
        public static CreateIndexDescriptor HaNoiRoadPointMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<HaNoiRoadPoint>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.id))
                .Text(t => t.Name(n => n.name))
                .Number(t => t.Name(n => n.lat))
                .Number(t => t.Name(n => n.lng))
                .GeoPoint(t => t.Name(n => n.location))
                //.GeoShape(t => t.Name(n ))
                )

            );
        }
    }
}
