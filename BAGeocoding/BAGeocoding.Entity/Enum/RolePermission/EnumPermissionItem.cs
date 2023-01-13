using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.RolePermission
{
    public enum EnumPermissionItem : int
    {
        #region ==================== 1. General (512: 1 -> 512) ====================
        #region ==================== 1.1 General - Data (64: 1 -> 64) ====================
        [EnumItem("Dữ liệu tỉnh/thành", FieldName = "GRNDataProvince")]
        GRNDataProvince = 1,

        [EnumItem("Dữ liệu quận/huyện", FieldName = "GRNDataDistrict")]
        GRNDataDistrict = 2,

        [EnumItem("Dữ liệu phường/xã", FieldName = "GRNDataCommune")]
        GRNDataCommune = 3,

        [EnumItem("Dữ liệu vùng tìm kiếm", FieldName = "GRNDataTile")]
        GRNDataTile = 4,

        [EnumItem("Dữ liệu đường phố", FieldName = "GRNDataSegment")]
        GRNDataSegment = 5,

        [EnumItem("Dữ liệu điểm", FieldName = "GRNDataPOI")]
        GRNDataPOI = 6,

        [EnumItem("Dữ liệu khu đô thị", FieldName = "GRNDataPlace")]
        GRNDataPlace = 7,

        [EnumItem("Dữ liệu tìm đường", FieldName = "GRNDataRoute")]
        GRNDataRoute = 8,
        #endregion

        #region ==================== 1.2 General - Other (32: 65 -> 96) ====================
        [EnumItem("Dữ liệu đi làm địa chỉ", FieldName = "GRNIExtendSeg4Address")]
        GRNIExtendSeg4Address = 65,

        [EnumItem("Dữ liệu đường polyline", FieldName = "GRNIExtendPolyline")]
        GRNIExtendPolyline = 66,
        #endregion

        #region ==================== 1.3 General - Convert (32: 97 -> 128) ====================
        [EnumItem("Dữ liệu đi làm địa chỉ", FieldName = "GRNConvertPointText2Mif")]
        GRNConvertPointText2Mif = 97,

        [EnumItem("Dữ liệu đường polyline", FieldName = "GRNConvertPolylineText2Mif")]
        GRNConvertPolylineText2Mif = 98,

        [EnumItem("Dữ liệu đường polyline", FieldName = "GRNConvertText2MifGps")]
        GRNConvertText2MifGps = 99,
        #endregion


        #region ==================== 1.y General - Write (64: 385 -> 448) ====================
        [EnumItem("Dữ liệu hành chính", FieldName = "GRNWriteRegion")]
        GRNWriteRegion = 385,

        [EnumItem("Dữ liệu khu đô thị", FieldName = "GRNWritePlace")]
        GRNWritePlace = 386,

        [EnumItem("Dữ liệu đường, số nhà", FieldName = "GRNWriteSegment")]
        GRNWriteSegment = 387,

        [EnumItem("Dữ liệu từ khóa tìm kiếm", FieldName = "GRNWriteSearch")]
        GRNWriteSearch = 388,

        [EnumItem("Dữ liệu tìm đường đi", FieldName = "GRNWriteRoute")]
        GRNWriteRoute = 389,
        #endregion

        #region ==================== 1.z General - Utility (64: 449 -> 512) ====================
        #endregion
        #endregion

        #region ==================== 2. Test (512: 513 -> 1028) ====================
        #region ==================== 2.1 Test - Data (64: 513 -> 64) ====================
        [EnumItem("Danh sách tỉnh/thành", FieldName = "TESTDataProvince")]
        TESTDataProvince = 513,

        [EnumItem("Danh sách quận/huyện", FieldName = "TESTDataDistrict")]
        TESTDataDistrict = 514,

        [EnumItem("Danh sách phường/xã", FieldName = "TESTDataCommune")]
        TESTDataCommune = 515,



        [EnumItem("Tên đường đặc biệt", FieldName = "TESTDataRoadSpecial")]
        TESTDataRoadSpecial = 520,
        #endregion

        #region ==================== 2.2 Test - Service (64: 577 -> 640) ====================
        [EnumItem("Dịch vụ địa chỉ", FieldName = "TESTServiceAddress")]
        TESTServiceAddress = 577,

        [EnumItem("Dữ liệu địa chỉ", FieldName = "TESTServiceDataCheck")]
        TESTServiceDataCheck = 578,

        [EnumItem("Dịch vụ hiển thị", FieldName = "TESTServiceImages")]
        TESTServiceImages = 579,
        #endregion
        #endregion


        #region ==================== 3. MapTool (512: 1029 -> 1540) ====================
        #region ==================== 3.1 MapTool - Data (64: 1029 -> 1092) ====================
        [EnumItem("Thông tin người dùng", FieldName = "MTLDataUser")]
        MTLDataUser = 1029,

        [EnumItem("Danh mục lưới làm việc", FieldName = "MTLDataGrid")]
        MTLDataGrid = 1030,

        [EnumItem("Danh mục loại điểm", FieldName = "MTLDataPOIKind")]
        MTLDataPOIKind = 1031,

        [EnumItem("Kế hoạch làm bản đồ", FieldName = "MTLDataPlan")]
        MTLDataPlan = 1032,

        [EnumItem("Tracklog di chuyển", FieldName = "MTLDataTrackMove")]
        MTLDataTrackMove = 1033,

        [EnumItem("Danh mục thiết bị iPad", FieldName = "MTLDataIPad")]
        MTLDataIPad = 1034,

        [EnumItem("Cấu trúc bảng thuộc tính", FieldName = "MTLDataFieldStructure")]
        MTLDataFieldStructure = 1035,


        [EnumItem("Phiên bản phần mềm", FieldName = "MTLDataVersionApp")]
        MTLDataVersionApp = 1085
        #endregion
        #endregion
    }
}