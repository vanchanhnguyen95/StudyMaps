using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticHelper
{
    public class Product
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int price { get; set; }
        public int isPopularProduct { get; set; }
        public string? category { get; set; }
        public string? keyWords { get; set; }
    }
}
