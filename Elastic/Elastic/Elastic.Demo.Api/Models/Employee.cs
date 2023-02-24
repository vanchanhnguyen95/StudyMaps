using Nest;
using System.ComponentModel;

namespace Elastic.Demo.Api.Models
{
    /// <summary>
    /// IdProperty for elasticsearchType will override default property generation by elastic search and will use 
    /// assigned property as id for document level. 
    /// Description holds the index name
    /// NOTE: Id value should be unique and index name should be in Lower Case
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Id)), Description("employee")]
    public class Employee
    {
        public Employee(string id)
        => Id = id;

        public Employee()
        {

        }

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// If text field needs to be sortable then Fielddata should set to TRUE 
        /// and by default if there is no mapping defined then elasticsearch will 
        /// use the standard analyzer which will result as case insensitive 
        /// search then output will not as expected. To overcome this behaviour 
        /// we have defined custom Analyzer which will override default one.
        /// Index true will make property to available for search, 
        /// Fielddata true for sorting, aggregations, or scripting.
        /// </summary> 
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Name { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Email { get; set; }
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string PhoneNumber { get; set; }
    }
}
