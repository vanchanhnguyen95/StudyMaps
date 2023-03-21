using Elastic02.Models.Test;
using Nest;
using System.ComponentModel;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Elastic02.Services.Test
{
    public class RoadNameService : IRoadNameService
    {
        public int NumberOfShards { get; set; } = 5;
        public int NumberOfReplicas { get; set; } = 1;
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly IConfiguration _configuration;
        IVietNamShapeService _vnShapeService;

        private string GetIndexName()
        {
            var type = typeof(RoadName);

            var customAttributes = (DescriptionAttribute[])type
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
                return customAttributes[0].Description;

            throw new Exception($"{nameof(RoadName)} description attribute is missing.");
        }

        public RoadNameService(ElasticClient client, IConfiguration configuration, IVietNamShapeService vnShapeService)
        {
            _client = client;
            _indexName = GetIndexName();
            _configuration = configuration;
            _vnShapeService = vnShapeService;
        }

        public async Task<string> BulkAsyncMultiProvince(List<RoadNamePush> roadPushs)
        {
            try
            {
                if (!roadPushs.Any())
                    return "Success - No data bulk";

                List<RoadName> roads = new List<RoadName>();
                List<RoadName> roadHaGiang = new List<RoadName>();//1
                List<RoadName> roadCaoBang = new List<RoadName>();
                List<RoadName> roadLaiChau = new List<RoadName>();
                List<RoadName> roadLaoCai = new List<RoadName>();
                List<RoadName> roadTuyenQuang = new List<RoadName>();
                List<RoadName> roadBacKan = new List<RoadName>();
                List<RoadName> roadLangSon = new List<RoadName>();
                List<RoadName> roadDienBien = new List<RoadName>();
                List<RoadName> roadYenBai = new List<RoadName>();
                List<RoadName> roadThaiNguyen = new List<RoadName>();//10

                List<RoadName> roadSonLa = new List<RoadName>();//11
                List<RoadName> roadPhuTho = new List<RoadName>();
                List<RoadName> roadVinhPhuc = new List<RoadName>();
                List<RoadName> roadBacGiang = new List<RoadName>();
                List<RoadName> roadQuangNinh = new List<RoadName>();
                List<RoadName> roadHaNoi = new List<RoadName>();
                List<RoadName> roadBacNinh = new List<RoadName>();
                List<RoadName> roadHungYen = new List<RoadName>();
                List<RoadName> roadHaiDuong = new List<RoadName>();
                List<RoadName> roadHaiPhong = new List<RoadName>();//20

                List<RoadName> roadHoaBinh = new List<RoadName>();//21
                List<RoadName> roadHaNam = new List<RoadName>();
                List<RoadName> roadThaiBinh= new List<RoadName>();
                List<RoadName> roadNinhBinh= new List<RoadName>();
                List<RoadName> roadNamDinh= new List<RoadName>();
                List<RoadName> roadThanhHoa= new List<RoadName>();
                List<RoadName> roadNgheAn= new List<RoadName>();
                List<RoadName> roadHaTinh= new List<RoadName>();
                List<RoadName> roadQuangBinh= new List<RoadName>();
                List<RoadName> roadQuangTri= new List<RoadName>();//30

                List<RoadName> roadThuaThienHue= new List<RoadName>();//31
                List<RoadName> roadDaNang= new List<RoadName>();
                List<RoadName> roadQuangNam= new List<RoadName>();
                List<RoadName> roadKonTum= new List<RoadName>();
                List<RoadName> roadQuangNgai= new List<RoadName>();
                List<RoadName> roadGiaLai= new List<RoadName>();
                List<RoadName> roadBinhDinh= new List<RoadName>();
                List<RoadName> roadPhuYen= new List<RoadName>();
                List<RoadName> roadDaklak= new List<RoadName>();
                List<RoadName> roadKhanhHoa= new List<RoadName>();//40

                List<RoadName> roadDakNong= new List<RoadName>();//41
                List<RoadName> roadBinhPhuoc= new List<RoadName>();
                List<RoadName> roadLamDong = new List<RoadName>();
                List<RoadName> roadNinhThuan = new List<RoadName>();
                List<RoadName> roadTayNinh = new List<RoadName>();
                List<RoadName> roadBinhDuong = new List<RoadName>();
                List<RoadName> roadDongNai = new List<RoadName>();
                List<RoadName> roadBinhThuan = new List<RoadName>();
                List<RoadName> roadLongAn = new List<RoadName>();
                List<RoadName> roadHoChiMinh = new List<RoadName>();//50

                List<RoadName> roadVungTau = new List<RoadName>();//51
                List<RoadName> roadAnGiang = new List<RoadName>();
                List<RoadName> roadDongThap = new List<RoadName>();
                List<RoadName> roadTienGiang = new List<RoadName>();
                List<RoadName> roadKienGiang = new List<RoadName>();
                List<RoadName> roadCanTho = new List<RoadName>();
                List<RoadName> roadVinhLong = new List<RoadName>();
                List<RoadName> roadBenTre = new List<RoadName>();
                List<RoadName> roadHauGiang = new List<RoadName>();
                List<RoadName> roadSocTrang = new List<RoadName>();//60
                List<RoadName> roadTraVinh = new List<RoadName>();
                List<RoadName> roadBacLieu = new List<RoadName>();
                List<RoadName> roadCaMau = new List<RoadName>();

                // Phân chia dữ liệu từng tỉnh
                foreach (var road in roadPushs)
                {
                    switch (road.ProvinceID)
                    {
                        case 1:
                            roadHaGiang.Add(new RoadName(road));
                            break;
                        case 2:
                            roadCaoBang.Add(new RoadName(road));
                            break;
                        case 3:
                            roadLaiChau.Add(new RoadName(road));
                            break;
                        case 4:
                            roadLaoCai.Add(new RoadName(road));
                            break;
                        case 5:
                            roadTuyenQuang.Add(new RoadName(road));
                            break;
                        case 6:
                            roadBacKan.Add(new RoadName(road));
                            break;
                        case 7:
                            roadLangSon.Add(new RoadName(road));
                            break;
                        case 8:
                            roadDienBien.Add(new RoadName(road));
                            break;
                        case 9:
                            roadYenBai.Add(new RoadName(road));
                            break;
                        case 10:
                            roadThaiNguyen.Add(new RoadName(road));
                            break;
                        case 11:
                            roadSonLa.Add(new RoadName(road));
                            break;
                        case 12:
                            roadPhuTho.Add(new RoadName(road));
                            break;
                        case 13:
                            roadVinhPhuc.Add(new RoadName(road));
                            break;
                        case 14:
                            roadBacGiang.Add(new RoadName(road));
                            break;
                        case 15:
                            roadQuangNinh.Add(new RoadName(road));
                            break;
                        case 16:
                            roadHaNoi.Add(new RoadName(road));
                            break;
                        case 17:
                            roadBacNinh.Add(new RoadName(road));
                            break;
                        case 18:
                            roadHungYen.Add(new RoadName(road));
                            break;
                        case 19:
                            roadHaiDuong.Add(new RoadName(road));
                            break;
                        case 20:
                            roadHaiPhong.Add(new RoadName(road));
                            break;

                        case 21:
                            roadHoaBinh.Add(new RoadName(road));
                            break;
                        case 22:
                            roadHaNam.Add(new RoadName(road));
                            break;
                        case 23:
                            roadThaiBinh.Add(new RoadName(road));
                            break;
                        case 24:
                            roadNinhBinh.Add(new RoadName(road));
                            break;
                        case 25:
                            roadNamDinh.Add(new RoadName(road));
                            break;
                        case 26:
                            roadThanhHoa.Add(new RoadName(road));
                            break;
                        case 27:
                            roadNgheAn.Add(new RoadName(road));
                            break;
                        case 28:
                            roadHaTinh.Add(new RoadName(road));
                            break;
                        case 29:
                            roadQuangBinh.Add(new RoadName(road));
                            break;
                        case 30:
                            roadQuangTri.Add(new RoadName(road));
                            break;
                        case 31:
                            roadThuaThienHue.Add(new RoadName(road));
                            break;
                        case 32:
                            roadDaNang.Add(new RoadName(road));
                            break;
                        case 33:
                            roadQuangNam.Add(new RoadName(road));
                            break;
                        case 34:
                            roadKonTum.Add(new RoadName(road));
                            break;
                        case 35:
                            roadQuangNgai.Add(new RoadName(road));
                            break;
                        case 36:
                            roadGiaLai.Add(new RoadName(road));
                            break;
                        case 37:
                            roadBinhDinh.Add(new RoadName(road));
                            break;
                        case 38:
                            roadPhuYen.Add(new RoadName(road));
                            break;
                        case 39:
                            roadDaklak.Add(new RoadName(road));
                            break;
                        case 40:
                            roadKhanhHoa.Add(new RoadName(road));
                            break;

                        case 41:
                            roadDakNong.Add(new RoadName(road));
                            break;
                        case 42:
                            roadBinhPhuoc.Add(new RoadName(road));
                            break;
                        case 43:
                            roadLamDong.Add(new RoadName(road));
                            break;
                        case 44:
                            roadNinhThuan.Add(new RoadName(road));
                            break;
                        case 45:
                            roadTayNinh.Add(new RoadName(road));
                            break;
                        case 46:
                            roadBinhDuong.Add(new RoadName(road));
                            break;
                        case 47:
                            roadDongNai.Add(new RoadName(road));
                            break;
                        case 48:
                            roadBinhThuan.Add(new RoadName(road));
                            break;
                        case 49:
                            roadLongAn.Add(new RoadName(road));
                            break;
                        case 50:
                            roadHoChiMinh.Add(new RoadName(road));
                            break;

                        case 51:
                            roadVungTau.Add(new RoadName(road));
                            break;
                        case 52:
                            roadAnGiang.Add(new RoadName(road));
                            break;
                        case 53:
                            roadDongThap.Add(new RoadName(road));
                            break;
                        case 54:
                            roadTienGiang.Add(new RoadName(road));
                            break;
                        case 55:
                            roadKienGiang.Add(new RoadName(road));
                            break;
                        case 56:
                            roadCanTho.Add(new RoadName(road));
                            break;
                        case 57:
                            roadVinhLong.Add(new RoadName(road));
                            break;
                        case 58:
                            roadBenTre.Add(new RoadName(road));
                            break;
                        case 59:
                            roadHauGiang.Add(new RoadName(road));
                            break;
                        case 60:
                            roadSocTrang.Add(new RoadName(road));
                            break;

                        case 61:
                            roadTraVinh.Add(new RoadName(road));
                            break;
                        case 62:
                            roadBacLieu.Add(new RoadName(road));
                            break;
                        case 63:
                            roadCaMau.Add(new RoadName(road));
                            break;
                    }
                }

                // Bulk insert dữ liệu từng tỉnh
                if (roadHaGiang.Any())//1
                    await BulkAsyncByProvince(roadHaGiang, "hagiang");

                if (roadCaoBang.Any())
                    await BulkAsyncByProvince(roadCaoBang, "caobang");

                if (roadLaiChau.Any())
                    await BulkAsyncByProvince(roadLaiChau, "laichau");

                if (roadLaoCai.Any())
                    await BulkAsyncByProvince(roadLaoCai, "laocai");

                if (roadTuyenQuang.Any())
                    await BulkAsyncByProvince(roadTuyenQuang, "tuyenquang");

                if (roadBacKan.Any())
                    await BulkAsyncByProvince(roadBacKan, "backan");

                if (roadLangSon.Any())
                    await BulkAsyncByProvince(roadLangSon, "langson");

                if (roadDienBien.Any())
                    await BulkAsyncByProvince(roadDienBien, "dienbien");

                if (roadYenBai.Any())
                    await BulkAsyncByProvince(roadYenBai, "yenbai");

                if (roadThaiNguyen.Any())//10
                    await BulkAsyncByProvince(roadThaiNguyen, "thainguyen");

                if (roadSonLa.Any())//11
                    await BulkAsyncByProvince(roadSonLa, "sonla");

                if (roadPhuTho.Any())
                    await BulkAsyncByProvince(roadPhuTho, "phutho");

                if (roadVinhPhuc.Any())
                    await BulkAsyncByProvince(roadVinhPhuc, "vinhphuc");

                if (roadBacGiang.Any())
                    await BulkAsyncByProvince(roadBacGiang, "bacgiang");

                if (roadQuangNinh.Any())
                    await BulkAsyncByProvince(roadQuangNinh, "quangninh");

                if (roadHaNoi.Any())
                    await BulkAsyncByProvince(roadHaNoi, "hanoi");

                if (roadBacNinh.Any())
                    await BulkAsyncByProvince(roadBacNinh, "bacninh");

                if (roadHungYen.Any())
                    await BulkAsyncByProvince(roadHungYen, "hungyen");

                if (roadHaiDuong.Any())
                    await BulkAsyncByProvince(roadHaiDuong, "haiduong");

                if (roadHaiPhong.Any())
                    await BulkAsyncByProvince(roadHaiPhong, "haiphong");//20

              

                return "Success";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        public async Task<string> BulkAsync(List<RoadNamePush> roadPushs)
        {
            List<RoadName> roads = new List<RoadName>();

            if (!roadPushs.Any())
                return "Success - No data to bulk insert";
            
            roadPushs.ForEach(item => roads.Add(new RoadName(item)));

            //Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
             await CreateIndex(_indexName);

            var bulkAllObservable = _client.BulkAll(roads, b => b
                .Index(_indexName)
                // how long to wait between retries
                .BackOffTime("30s")
                // how many retries are attempted if a failure occurs
                .BackOffRetries(2)
                // refresh the index once the bulk operation completes
                .RefreshOnCompleted()
                // how many concurrent bulk requests to make
                .MaxDegreeOfParallelism(Environment.ProcessorCount)
                // number of items per bulk request
                .Size(10000)
                // decide if a document should be retried in the event of a failure
                //.RetryDocumentPredicate((item, road) =>
                //{
                //    return item.Error.Index == "even-index" && person.FirstName == "Martijn";
                //})
                // if a document cannot be indexed this delegate is called
                .DroppedDocumentCallback(async (bulkResponseItem, road) =>
                {
                    bool isCreate = true;
                    //isCreate = await CreateAsync(road, _indexName);
                    isCreate = await IndexAsync(road, _indexName);
                    while (isCreate == false)
                    {
                        //isCreate = await IndexAsync(road, _indexName);
                        isCreate = await IndexAsync(road, _indexName);
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine($"{road.ProvinceID} - {road.RoadName}");
                    }
                })
                .Timeout(Environment.ProcessorCount)
                .ContinueAfterDroppedDocuments()
            )
            .Wait(TimeSpan.FromMinutes(15), next =>
            {
                // do something e.g. write number of pages to console
            });

            return "Success";
        }

        public async Task<string> BulkAsyncByProvince(List<RoadName> roads, string indexName = null)
        {
            try
            {
                if (!roads.Any())
                    return "Success - No data bulk";

                // Check xem khởi tạo index chưa, nếu chưa khởi tạo thì phải khởi tạo index mới được
                await CreateIndex(indexName);

                var bulkAllObservable = _client.BulkAll(roads, b => b
                    .Index(indexName)
                    // how long to wait between retries
                    .BackOffTime("30s")
                    // how many retries are attempted if a failure occurs
                    .BackOffRetries(2)
                    // refresh the index once the bulk operation completes
                    .RefreshOnCompleted()
                    // how many concurrent bulk requests to make
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    // number of items per bulk request
                    .Size(20000)
                    // decide if a document should be retried in the event of a failure
                    //.RetryDocumentPredicate((item, road) =>
                    //{
                    //    return item.Error.Index == "even-index" && person.FirstName == "Martijn";
                    //})
                    // if a document cannot be indexed this delegate is called
                    .DroppedDocumentCallback(async (bulkResponseItem, road) =>
                    {
                        bool isCreate = true;
                        isCreate = await CreateAsync(road, indexName);
                        while (isCreate == false)
                        {
                            isCreate = await IndexAsync(road, indexName);
                            Console.OutputEncoding = Encoding.UTF8;
                            Console.WriteLine($"{road.RoadName}");
                        }
                    })
                    .Timeout(Environment.ProcessorCount)
                    .ContinueAfterDroppedDocuments()
                )
                .Wait(TimeSpan.FromMinutes(15), next =>
                {
                    // do something e.g. write number of pages to console
                });

                return "Success";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, GeoDistanceType type, string distance, int size, string keyword, GeoShapeRelation relation)
        {
            try
            {
                List<RoadName> res = new List<RoadName>();
                List<RoadNamePush> result = new List<RoadNamePush>();

                //res = await GetDataByKeyWord(size, keyword);

                //Tìm kiếm theo tọa độ
                if (string.IsNullOrEmpty(keyword))
                {
                    int provinceid = await GetProvinceId(lat, lng, null);
                    res = await GetDataByLocation(lat, lng, distance, size, provinceid);
                }
                // Tìm kiếm theo từ khóa
                else if (lat == 0 && lng == 0)
                {
                    res = await GetDataByKeyWord(size, keyword);
                }
                // Tìm kiếm theo tọa độ và từ khóa
                else
                {
                    int provinceid = await GetProvinceId(lat, lng, null);
                    res = await GetDataByLocationKeyWord(lat, lng, distance, size, keyword, provinceid);
                }

                if (res.Any())
                    res.ForEach(item => result.Add(new RoadName(item)));

                return result;
                //return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<RoadNamePush>> GetDataSuggestion(double lat, double lng, string distance, int size, string keyword)
        {
            List<RoadName> res = new List<RoadName>();
            List<RoadNamePush> result = new List<RoadNamePush>();

            int provinceid = 0;

            if (lat != 0 && lng != 0)
                provinceid = await GetProvinceId(lat, lng,null);

            // Tìm kiếm theo tọa độ
            if (string.IsNullOrEmpty(keyword))
            {
                res = await GetDataByLocation(lat, lng, distance, size, provinceid);
            }
            // Tìm kiếm theo từ khóa
            else if (lat == 0 && lng == 0)
            {
                res = await GetDataByKeyWord(size, keyword, provinceid);
            }
            // Tìm kiếm theo tọa độ và từ khóa
            else
            {
                res = await GetDataByLocationKeyWord(lat, lng, distance, size, keyword, provinceid);
            }

            if (res.Any())
                res.ForEach(item => result.Add(new RoadName(item)));

            return result;
        }

        // Tìm kiếm theo tọa độ
        private async Task<List<RoadName>> GetDataByLocation(double lat, double lng, string distance, int size)
        {
            try
            {
                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Sort(s => s
                        //.Ascending(f => f.NameExt)
                        .Ascending(SortSpecialField.Score)
                        .GeoDistance(
                        d => d
                        //.Field(f => f.Location)
                        .Points(new GeoLocation(lat, lng))
                        .Order(SortOrder.Ascending)
                        .Unit(DistanceUnit.Kilometers)
                        .Mode(SortMode.Min)
                        .DistanceType(GeoDistanceType.Arc)
                        .IgnoreUnmapped()
                        )
                   )
                   .PostFilter(q => q.GeoDistance(
                        g => g
                        .Distance(distance)
                        .DistanceType(GeoDistanceType.Arc)
                        .Location(lat, lng)
                        .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        .IgnoreUnmapped()
                        .Boost(1.1)
                        .Name("named_query")
                        //.Field(p => p.Location)
                        
                    ))
                   
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<RoadName>> GetDataByLocation(double lat, double lng, string distance, int size, int provinceID)
        {
            try
            {
                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                       b => b.Must(mu => mu.Match(ma =>
                            ma.Field(f => f.ProvinceID).Query(provinceID.ToString())
                       )
                    ))
                   )
                   .PostFilter(q => q.GeoDistance(
                        g => g
                        .Distance(distance)
                        .DistanceType(GeoDistanceType.Arc)
                        .Location(lat, lng)
                        .ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                        .IgnoreUnmapped()
                        .Boost(1.1)
                        .Name("named_query")
                        .Field(p => p.Location)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score).Ascending(f => f.NameExt)
                            .GeoDistance(
                                        d => d
                                        .Field(f => f.Location)
                                        .Order(SortOrder.Ascending).Points(new GeoLocation(lat, lng))
                                        .Unit(DistanceUnit.Meters).Mode(SortMode.Min)
                                        )
                   )
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch
            {
                return new List<RoadName>();
            }
        }

        // Tìm kiếm theo từ khóa
        private async Task<List<RoadName>> GetDataByKeyWord(int size, string keyword)
        {
            try
            {
                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        //ma.Field(f => f.Keywords).Analyzer("my_combined_analyzer").Analyzer("keyword_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)
                        ma.Field(f => f.KeywordsAscii).Analyzer("vn_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        //&& mu.Match(ma =>
                        //ma.Field(f => f.RoadName).Analyzer("vn_analyzer").Query(keyword)
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                    )))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   //.Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<RoadName>> GetDataByKeyWord(int size, string keyword, int provinceID)
        {
            try
            {
                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.Keywords).Analyzer("vn_analyzer").Query(keyword).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery()
                        )
                        //&& mu.Match(ma =>
                        //ma.Field(f => f.RoadName).Analyzer("vn_analyzer").Query(keyword)
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                        && mu.Match(ma =>
                        ma.Field(f => f.ProvinceID).Query(provinceID.ToString())
                        )
                    )))
                   .Sort(s => s.Descending(SortSpecialField.Score).Ascending(f => f.NameExt)
                   )
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Tìm kiếm theo tọa độ và từ khóa
        private async Task<List<RoadName>> GetDataByLocationKeyWord(double lat, double lng, string distance, int size, string keyword)
        {
            try
            {
                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.KeywordsAscii).Query(keyword).Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery())
                        //&& mu.Match(ma =>
                        //ma.Field(f => f.RoadName).Query(keyword)
                        //.AutoGenerateSynonymsPhraseQuery())
                    )))
                   .PostFilter(q => q.GeoDistance(
                        g => g.Boost(1.1).Name("named_query")
                        .Field(p => p.Location)
                        .DistanceType(GeoDistanceType.Arc).Location(lat, lng)
                        .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<RoadName>> GetDataByLocationKeyWord(double lat, double lng, string distance, int size, string keyword, int provinceID)
        {
            try
            {
                var geo = await _client.SearchAsync<RoadName>(s => s.Index(_indexName)
                   .Size(size)
                   .Query(q => q.Bool(
                        b => b.Must(mu => mu.Match(ma =>
                        ma.Field(f => f.KeywordsAscii).Query(keyword).Analyzer("vn_analyzer").Fuzziness(Fuzziness.Auto)
                        .AutoGenerateSynonymsPhraseQuery())
                        //&& mu.Match(ma =>
                        //ma.Field(f => f.RoadName).Query(keyword)
                        //.AutoGenerateSynonymsPhraseQuery()
                        //)
                        && mu.Match(ma =>
                        ma.Field(f => f.ProvinceID).Query(provinceID.ToString()))

                    )))
                   .PostFilter(q => q.GeoDistance(
                        g => g.Boost(1.1).Name("named_query")
                        .Field(p => p.Location)
                        .DistanceType(GeoDistanceType.Arc).Location(lat, lng)
                        .Distance(distance).ValidationMethod(GeoValidationMethod.IgnoreMalformed)
                    ))
                   .Sort(s => s.Descending(SortSpecialField.Score))
                   .Scroll(1)
                   );

                return geo.Documents.ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> CreateIndex(string indexName)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                await _client.Indices.DeleteAsync(Indices.Index(indexName));

                var indexResponse = await _client.Indices.CreateAsync(Indices.Index(indexName), c => c
               .Map<RoadName>(mm => mm.AutoMap())
               .Settings(s => s
                   .NumberOfReplicas(NumberOfReplicas)
                   .NumberOfShards(NumberOfShards)
                   .Analysis(a => a
                       .CharFilters(cf => cf
                           .Mapping("programming_language", mca => mca
                               .Mappings(new[]
                               {
                                        "c# => csharp",
                                        "C# => Csharp"
                               })
                           )
                         )
                       .CharFilters(cf => cf
                           .Mapping("province_name", mca => mca
                               .Mappings(new[]
                               {
                                    "hà nội => ha noi",
                                    "Hà Nội => Ha Noi"
                               })
                           )
                         )
                       .TokenFilters(tf => tf
                           .AsciiFolding("ascii_folding", tk => new AsciiFoldingTokenFilter
                           {
                               PreserveOriginal = true
                           })
                           .Synonym("synonym_address", sf => new SynonymTokenFilter
                           {
                               Synonyms = new List<string>()
                               { "ha noi, hà nội, Hà Nội, Ha Noi, hn, hanoi, tp. ha noi, thành phố ha noi",
                                    "tphcm,tp.hcm,tp hồ chí minh,sài gòn, saigon"
                               }
                           })
                       )
                       .Analyzers(an => an
                           .Custom("keyword_analyzer", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("keyword")
                               .Filters("lowercase"))
                           .Custom("my_combined_analyzer", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("vi_tokenizer")
                               .Filters("lowercase", "stop", "ascii_folding")
                           )
                           .Custom("vi_analyzer2", ca => ca
                                .CharFilters("province_name")
                                .Tokenizer("vi_tokenizer")
                                .Filters("lowercase", "ascii_folding")
                            )
                           .Custom("vn_analyzer", ca => ca
                               .CharFilters("html_strip")
                               .Tokenizer("vi_tokenizer")
                               .Filters("lowercase", "ascii_folding")
                           )
                           .Custom("address_analyzer", ca => ca
                               //.CharFilters("html_strip", "province_name")
                               .Tokenizer("vi_tokenizer")
                               .Filters("synonym_address","lowercase", "ascii_folding")
                           )
                       )
                   )
                )
            );

                return indexResponse.ApiCall.HttpStatusCode.ToString() ?? "OK";
            }
            catch (Exception ex) { return ex.ToString(); }
        }

        private async Task<bool> CreateAsync(RoadName item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.CreateAsync(new RoadName(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<RoadName>(item.RoadID.ToString(), a => a.Index(indexName).Doc(new RoadName(item)));
                }
                return response.IsValid;
            }
            catch (Exception ex) { return false; }

        }

        private async Task<bool> IndexAsync(RoadName item, string indexName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(indexName)) indexName = _indexName;

                var response = await _client.IndexAsync(new RoadName(item), q => q.Index(indexName));
                if (response.ApiCall?.HttpStatusCode == 409)
                {
                    await _client.UpdateAsync<RoadName>(item.RoadID.ToString(), a => a.Index(indexName).Doc(new RoadName(item)));
                }
                return response.IsValid;
            }
            catch { return false; }

        }

        private async Task<int> GetProvinceId(double lat = 0, double lng = 0, string keyword = null)
        {
            List<VietNamShape> lst;
                lst = await _vnShapeService.GetDataSuggestion(lat, lng, GeoDistanceType.Arc, "50km", 5, null, GeoShapeRelation.Intersects);

            if (lst.Any())
                return lst.Where(x => x.provinceid > 0).Select(x => x.provinceid).FirstOrDefault();

            return 16;//Hà Nội
        }

    }
}
