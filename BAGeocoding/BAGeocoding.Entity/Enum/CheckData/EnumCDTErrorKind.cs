using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.CheckData
{
    public enum EnumCDTErrorKind : byte
    {
        [EnumItem("Thiếu số nhà", FieldName = "Missing")]
        Missing = 1,

        [EnumItem("Trùng số nhà", FieldName = "Duplicate")]
        Duplicate = 2
    }
}