using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.RolePermission
{
    public enum EnumContextMenuItem : int
    {
        #region ================ Tài khoản người dùng (256: 65537 -> 65792) ================
        [EnumItem("Cập nhật mật khẩu", FieldName = "MTLUSRPassword")]
        MTLUSRPassword = 65537,

        [EnumItem("Phân quyền người dùng", FieldName = "MTLUSRPermission")]
        MTLUSRPermission = 65538,

        [EnumItem("Phân quyền menu chức năng", FieldName = "MTLUSRContextMenu")]
        MTLUSRContextMenu = 65539,


        [EnumItem("Gán dữ liệu từ người dùng khác", FieldName = "MTLUSRAssignData")]
        MTLUSRAssignData = 65544,

        [EnumItem("Lịch sử các lần đăng nhập", FieldName = "MTLUSRUserSession")]
        MTLUSRUserSession = 65545,

        [EnumItem("Trạng thái xử lý các grid", FieldName = "MTLUSRPlanGridState")]
        MTLUSRPlanGridState = 65546,

        [EnumItem("Danh sách kế hoạch tham gia", FieldName = "MTLUSRPlanList")]
        MTLUSRPlanList = 65547,

        [EnumItem("Các lần đăng nhập lỗi", FieldName = "MTLUSRUserLoginError")]
        MTLUSRUserLoginError = 65548,
        #endregion

        #region ================ Danh sách grid (256: 65793 -> 66048) ================
        [EnumItem("Danh sách tài khoản gán grid", FieldName = "MTLGRDPlanUserGrid")]
        MTLGRDPlanUserGrid = 65793,

        [EnumItem("Reset lại trạng thái grid", FieldName = "MTLGRDPlanGridReset")]
        MTLGRDPlanGridReset = 65794,
        #endregion

        #region ================ Loại điểm (256: 66049 -> 66304) ================
        
        #endregion

        #region ================ Kế hoạch làm bản đồ (256: 66305 -> 66560) ================
        [EnumItem("Gán grid cho kế hoạch", FieldName = "MTLPLNAssignGrid")]
        MTLPLNAssignGrid = 66305,

        [EnumItem("Import dữ liệu bản đồ", FieldName = "MTLPLNImportData")]
        MTLPLNImportData = 66306,

        [EnumItem("Gán người thực hiện", FieldName = "MTLPLNAssignUser")]
        MTLPLNAssignUser = 66307,

        [EnumItem("Phân grid cho người dùng", FieldName = "MTLPLNUserGrid")]
        MTLPLNUserGrid = 66308,

        [EnumItem("Báo trạng thái kế hoạch", FieldName = "MTLPLNBehavior")]
        MTLPLNBehavior = 66309,

        [EnumItem("Export dữ liệu theo grid", FieldName = "MTLPLNExportData")]
        MTLPLNExportData = 66310,

        [EnumItem("Báo trạng thái grid", FieldName = "MTLPLNGridState")]
        MTLPLNGridState = 66311,

        [EnumItem("Duyệt dữ liệu điểm", FieldName = "MTLPLNApprovedPoint")]
        MTLPLNApprovedPoint = 66312,

        [EnumItem("Duyệt dữ liệu đường", FieldName = "MTLPLNApprovedSegment")]
        MTLPLNApprovedSegment = 66313,



        [EnumItem("Duyệt/Trả lại grid", FieldName = "MTLPLNGridAppGvb")]
        MTLPLNGridAppGvb = 66320,    //66313,

        [EnumItem("Dữ liệu tiện ích", FieldName = "MTLPLNDataUtility")]
        MTLPLNDataUtility = 66321,

        [EnumItem("Trạng thái grid", FieldName = "MTLPLNDTSGridState")]
        MTLPLNDTSGridState = 66322,

        [EnumItem("Dữ liệu di chuyển của người dùng", FieldName = "MTLPLNDTSTrackMove")]
        MTLPLNDTSTrackMove = 66323,

        [EnumItem("Dữ liệu của grid", FieldName = "MTLPLNDTSGridData")]
        MTLPLNDTSGridData = 66324,

        [EnumItem("Tính điểm cho nhân sự", FieldName = "MTLPLNDataUserMark")]
        MTLPLNDataUserMark = 66325,
        #endregion

        #region ================ Danh sách iPad (32: 66593 -> 66624) ================
        [EnumItem("Gán iPad cho người dùng", FieldName = "MTLIPDAssignUser")]
        MTLIPDAssignUser = 66593,

        [EnumItem("Lịch sử gán iPad cho người dùng", FieldName = "MTLIPDAssignHistory")]
        MTLIPDAssignHistory = 66594,

        [EnumItem("Lịch sử cập nhật phần mềm", FieldName = "MTLIPDUpdateApp")]
        MTLIPDUpdateApp = 66595,

        [EnumItem("Lịch sử người dùng đăng nhập", FieldName = "MTLIPDLoginHistory")]
        MTLIPDLoginHistory = 66596,
        #endregion

        #region ================ Danh sách tỉnh thành (32: 66625 -> 66656) ================
        [EnumItem("Khởi tạo danh sách tên đường", FieldName = "CTLPRVRoadName")]
        CTLPRVRoadName = 66625,

        [EnumItem("Xử lý dữ liệu tìm kiếm tên đường", FieldName = "CTLPRVElasticSearchRoad")]
        CTLPRVElasticSearchRoad = 66626,

        [EnumItem("Xử lý dữ liệu tìm kiếm theo điểm", FieldName = "CTLPRVElasticSearchPoint")]
        CTLPRVElasticSearchPoint = 66627,
        #endregion
    }
}