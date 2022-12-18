using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterWatch.Models;

namespace WaterWatch.Repositories
{
    public interface IWaterConsumptionRepository
    {
        Task<IEnumerable<WaterConsumption>> GetAll();
        Task<IEnumerable<WaterConsumption>> GetTopTenConsumers();
    }
}