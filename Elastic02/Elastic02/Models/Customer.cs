using Nest;
using System;
using System.ComponentModel;

namespace Elastic02.Models
{
    /// <summary>
    /// Any model which represents Elastic index should mark with Description attribute
    /// should hold the index name in LOWER CASE.
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Id)), Description("customer")]
    public class Customer
    {
        public Customer(string id) => Id = id;
        public Customer()
        {

        }

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// Index true will make property to available for search, 
        /// fielddata true for sorting, aggregations, or scripting,
        /// custom analyzer for casesensitive search
        /// </summary>
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Name { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string SecurityNo { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Company { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Country { get; set; }
        public AccountDetail Account { get; set; }
    }
}
