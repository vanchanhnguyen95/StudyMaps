using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.RolePermission
{
    public enum EnumContextMenuObject : short
    {
        #region ================ General (128: 1 -> 128) ================
        [EnumItem("Tỉnh thành", FieldName = "CTLProvince")]
        CTLProvince = 65,
        #endregion

        #region ================ Test (128: 129 -> 256) ================
        #endregion

        #region ================ MapTool (128: 257 -> 384) ================
        [EnumItem("Người dùng", FieldName = "MTLUser")]
        MTLUser = 257,

        [EnumItem("Lưới làm việc", FieldName = "MTLGrid")]
        MTLGrid = 258,

        [EnumItem("Loại điểm", FieldName = "MTLPKind")]
        MTLPKind = 259,

        [EnumItem("Kế hoạch", FieldName = "MTLPlan")]
        MTLPlan = 260,

        [EnumItem("iPad", FieldName = "MTLIPad")]
        MTLIPad = 261,
        #endregion
    }
}