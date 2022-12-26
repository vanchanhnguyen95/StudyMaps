using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspMongoApi.Models
{
    public class StudentsDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}