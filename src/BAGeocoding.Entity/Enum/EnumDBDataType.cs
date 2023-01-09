using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumDBDataType : byte
    {
        [EnumItem("Number", FieldName = "Number")]
        Number = 1,

        [EnumItem("Text", FieldName = "Text")]
        Text = 17
    }
}