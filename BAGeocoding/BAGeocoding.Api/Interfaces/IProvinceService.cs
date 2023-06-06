namespace BAGeocoding.Api.Interfaces;

public interface IProvinceService
{
    Task<string> CreateIndex(string indexName);

    Task<string> BulkAsync(List<Province> vietNamShapes);

    //Task<string> AddAsync(List<VietNamShape> vietNamShapes);
    // Tìm kiếm theo Tọa độ / Từ Khóa / Tọa độ và từ khóa
    Task<List<Province>> GetDataSuggestion(int size, string keyword);
    Task<string> GetProvinceId(string keyword);
}
