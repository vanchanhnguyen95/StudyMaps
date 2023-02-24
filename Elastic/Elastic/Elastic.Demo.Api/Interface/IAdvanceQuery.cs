using Elastic.Demo.Api.Models;

namespace Elastic.Demo.Api.Interface
{
    public interface IAdvanceQuery
    {
        Task<CommonStats> StatsAggregationAsync(string key, string fieldName);
        /// <summary>
        /// This method is just an example how to pull individual stats though all stats can be pull 
        /// using above StatsAggregationAsync method.
        /// </summary>
        Task<double> MedianAggregationAsync(string key, string fieldName);
        Task<List<GroupStats>> GroupByAsync(string fieldName);
    }
}
