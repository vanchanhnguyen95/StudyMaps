using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticProject.Data.Entity
{
    public class Cities
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public int Population { get; set; }

        
        //public string geoShapeBase { get; set; }
    }
}
