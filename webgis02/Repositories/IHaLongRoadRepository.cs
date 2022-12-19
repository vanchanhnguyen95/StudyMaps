using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webgis02.Models;

namespace webgis02.Repositories
{
    public interface IHaLongRoadRepository
    {
        Task<IEnumerable<HaLongRoad>> GetAll();
        Task<IEnumerable<HaLongRoad>> GetTopTenRoads();
    }
}