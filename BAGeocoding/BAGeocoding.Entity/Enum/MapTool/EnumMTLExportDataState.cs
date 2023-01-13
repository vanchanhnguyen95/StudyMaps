using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLExportDataState : byte
    {
        [EnumItem("Toàn bộ", FieldName = "Normal")]
        Normal = 1,

        [EnumItem("Khởi tạo", FieldName = "General")]
        General = 2
    }
}