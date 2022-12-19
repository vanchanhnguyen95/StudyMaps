using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webgis02.Models;

namespace webgis02.Data
{
    public interface IDataContext
    {
        DbSet<HaLongRoad> HaLongRoads {get;set;}

        int SaveChanges();
    }
}