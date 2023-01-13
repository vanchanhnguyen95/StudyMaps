using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLPlanUserDataExt : byte
    {
        [EnumItem("Phụ trách chính của kế hoạch", FieldName = "Main")]
        Main = 0,

        [EnumItem("Kế hoạch đang được triển khai", FieldName = "Work")]
        Work = 1
    }
}