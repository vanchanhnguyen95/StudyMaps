using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum
{
    public enum EnumDBFieldType : byte
    {
        [EnumItem("Short interger", FieldName = "ShortInterger")]
        ShortInterger = 1,

        [EnumItem("Long interger", FieldName = "LongInterger")]
        LongInterger = 2,


        [EnumItem("Double", FieldName = "Double")]
        Double = 5,


        [EnumItem("Text", FieldName = "Text")]
        Text = 17
    }
}