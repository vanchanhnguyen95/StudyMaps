using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterWatch.Models
{
    public class WaterConsumption
    {
        public int id {get;set;}
        public string neighbourhood {get;set;}
        public string suburb_group {get;set;}
        public int averageMonthlyKL {get;set;}
        public string coordinates {get;set;}
    }
}