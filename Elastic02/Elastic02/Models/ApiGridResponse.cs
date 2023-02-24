using System;
using System.Collections.Generic;

namespace Elastic02.Models
{
    public struct ApiGridResponse<T> where T : class
    {
        public ApiGridResponse(IEnumerable<T> data, long total)
        {
            Records = data;
            Total = total;
        }

        public IEnumerable<T> Records { get; set; }
        public long Total { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ApiGridResponse<T> response &&
                   EqualityComparer<IEnumerable<T>>.Default.Equals(Records, response.Records) &&
                   Total == response.Total;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Records, Total);
        }

        public override string ToString()
        {
            return $"No of Records: {Total}";
        }
    }
}
