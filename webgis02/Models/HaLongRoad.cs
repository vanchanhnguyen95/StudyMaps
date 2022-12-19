using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webgis02.Models
{
    public class HaLongRoad
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public short? ClassFunc {get;set;}
        public short? Level {get;set;}
        public short? Kind {get;set;}
        public short? Minspeed {get;set;}
        public short? MaxSpeed {get;set;}
        public short? ProvinceID {get;set;}
        public long? SegmentID { get; set; }
        public string Coordinates {get;set;}
    }
}