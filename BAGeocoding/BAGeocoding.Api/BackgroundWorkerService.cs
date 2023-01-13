using BAGeocoding.Api.Services;
using BAGeocoding.Bll;
using BAGeocoding.Utility;

namespace BAGeocoding.Api
{
    public class BackgroundWorkerService : BackgroundService
    {
        private ILogger<BackgroundWorkerService> _logger;
        private readonly IConfiguration _config;
        private RegionManagerService _region;
        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IConfiguration config, RegionManagerService region)
        {
            _logger = logger;
            _config = config;
            LoadConfig();
            _region = region;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var result = false;
            //while (!result)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    result = await InitDataAsync();
            //    _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
            //}
            var addBADistrict = false;
            while (!addBADistrict)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _region.AddBAGDistrict();
                _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
            }

        }

        private void LoadConfig()
        {
            Constants.DEFAULT_SPLIT_KEYS = _config.GetSection("SplitKeys").Get<char>();
            Constants.DEFAULT_SPLIT_DATA = _config.GetSection("SplitData").Get<char>();
#pragma warning disable CS8601 // Possible null reference assignment.
            Constants.DEFAULT_DIRECTORY_DATA = _config.GetSection("Data").Get<string>();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            Constants.DEFAULT_DIRECTORY_LOGS = _config.GetSection("Logs").Get<string>();
#pragma warning restore CS8601 // Possible null reference assignment.
            Constants.DISTANCE_INTERSECT_ROAD = DataUtl.ConvertMeterToLngLat(Convert.ToInt32(_config.GetSection("DistanceIntersectRoad").Get<string>()));
            Constants.DISTANCE_INTERSECT_EPSILON = DataUtl.ConvertMeterToLngLat(Convert.ToInt32(_config.GetSection("DistanceIntersectEpsilon").Get<string>()));
#pragma warning disable CS8604 // Possible null reference argument.
            DataUtl.ConvertDistanceIntersect(_config.GetSection("DistanceIntersectRange").Get<string>());
#pragma warning restore CS8604 // Possible null reference argument.
            RunningParams.TestSpeed = Convert.ToBoolean(_config.GetSection("ServiceTestSpeed").Get<string>());
            RunningParams.DataSpeed = Convert.ToBoolean(_config.GetSection("ServiceDataSpeed").Get<string>());
            RunningParams.CheckAuthen = Convert.ToBoolean(_config.GetSection("CheckAuthenticate").Get<string>());

#pragma warning disable CS8601 // Possible null reference assignment.
            RunningParams.GOOGLE_GEOCODE_KEY = _config.GetSection("GoogleKey").Get<string>();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            RunningParams.ProvinceRoadByLevel = _config.GetSection("RoadByLevel").Get<string>();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            RunningParams.DistrictPriorityStr = _config.GetSection("PriorityStr").Get<string>();
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private async Task<bool> InitDataAsync()
        {
            await Task.Delay(1000);
            LoadConfig();
            MainProcessing.InitData();
            return true;
        }
    }
}
