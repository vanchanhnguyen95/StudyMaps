using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webgis02.Models;
using webgis02.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace webgis02.Repositories
{
    public class HaLongRoadRepository : IHaLongRoadRepository
    {
        private readonly IDataContext _context;

        public HaLongRoadRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HaLongRoad>> GetAll()
        {
            SaveData();
            return await _context.HaLongRoads.ToListAsync();
        }

        public async Task<IEnumerable<HaLongRoad>> GetTopTenRoads()
        {
            var q = _context.HaLongRoads
            .OrderByDescending(x => x.ProvinceID)
            .Take(10)
            .ToListAsync();
            return await q;
        }

        public void SaveData()
        {
            // Check if the table is empty before we load the data else, skip the etract transform and load process
            var res_dataset = _context.HaLongRoads.ToList();

            if(res_dataset.Count() == 0)
            {
                Console.WriteLine("No data");
                var geoJson =File.ReadAllText(@"E:\Project\webgis02\HaLong_Road.geojson");
            
                dynamic jsonObj = JsonConvert.DeserializeObject(geoJson);

                foreach(var feature in jsonObj["features"])
                {
                    // Extract values from the file object using the fields
                    string str_Name = feature["properties"]["Name"];
                    string str_ClassFunc = feature["properties"]["ClassFunc"];
                    string str_Level = feature["properties"]["Level"];
                    string str_Kind = feature["properties"]["Kind"];
                    string str_Minspeed = feature["properties"]["Minspeed"];
                    string str_MaxSpeed = feature["properties"]["MaxSpeed"];
                    string str_ProvinceID = feature["properties"]["ProvinceID"];
                    string str_SegmentID = feature["properties"]["SegmentID"];
                    string str_geometry = feature["geometry"]["coordinates"].ToString(Newtonsoft.Json.Formatting.None);

                    // Apply Transformations

                    // Remove .0's from values
                    // string conv_avgMthlyKl = str_avgMonthlyKL.Replace(".0", "");
                    // Convert string to int
                    short classFunc = Convert.ToInt16(str_ClassFunc);
                    short level = Convert.ToInt16(str_Level);
                    short kind = Convert.ToInt16(str_Kind);
                    short minspeed = Convert.ToInt16(str_Minspeed);
                    short maxSpeed = Convert.ToInt16(str_MaxSpeed);
                    short provinceID = Convert.ToInt16(str_ProvinceID);
                    long segmentID = Convert.ToInt64(str_SegmentID);
                   

                    // Load the data into our table
                    HaLongRoad wc = new ()
                    {
                        Name = str_Name,
                        ClassFunc = classFunc,
                        Level = level,
                        Kind = kind,
                        Minspeed = minspeed,
                        MaxSpeed = maxSpeed,
                        ProvinceID = provinceID,
                        SegmentID = segmentID,
                        Coordinates = str_geometry
                    };

                    _context.HaLongRoads.Add(wc);

                    _context.SaveChanges();
                }
            
            }
            else
            {
                Console.WriteLine("Data Loaded");
            }


        }
    }
}