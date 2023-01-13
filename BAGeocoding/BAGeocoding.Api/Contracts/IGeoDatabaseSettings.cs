namespace BAGeocoding.Api.Contracts
{
    public interface IGeoDatabaseSettings
    {
        string BAGDistrictAddsCollectionName { get; set; }
        string ProvinceDatasCollectionName { get; set; }
        string DistrictDatasCollectionName { get; set; }
        string CommuneDatasCollectionName { get; set; }
        string TileDatasCollectionName { get; set; }
        string PlaceDatasCollectionName { get; set; }
        string PointDatasCollectionName { get; set; }
        string RouterDatasCollectionName { get; set; }
        string ROUTE_EPSILONsCollectionName { get; set; }
        string HTProvincePrioritysCollectionName { get; set; }
        string RoadSpecialsCollectionName { get; set; }
        string DistrictPrioritysCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
