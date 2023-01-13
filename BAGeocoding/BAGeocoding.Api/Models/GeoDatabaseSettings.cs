namespace BAGeocoding.Api.Models
{
    public class GeoDatabaseSettings
    {
        public string? BAGDistrictAddsCollectionName { get; set; } = null!;
        public string? ProvinceDatasCollectionName { get; set; } = null!;
        public string? DistrictDatasCollectionName { get; set; } = null!;
        public string? CommuneDatasCollectionName { get; set; } = null!;
        public string? TileDatasCollectionName { get; set; } = null!;
        public string? PlaceDatasCollectionName { get; set; } = null!;
        public string? PointDatasCollectionName { get; set; } = null!;
        public string? RouterDatasCollectionName { get; set; }
        public string? ROUTE_EPSILONsCollectionName { get; set; }
        public string? HTProvincePrioritysCollectionName { get; set; }
        public string? RoadSpecialsCollectionName { get; set; }
        public string? DistrictPrioritysCollectionName { get; set; }

        public string? ConnectionString { get; set; } = null!;
        public string? DatabaseName { get; set; } = null!;

    }
}
