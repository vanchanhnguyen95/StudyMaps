using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLAppType : byte
    {
        [EnumItem("Window", FieldName = "Window")]
        Window = 1,

        [EnumItem("Mobile", FieldName = "Mobile")]
        Mobile = 2,

        [EnumItem("Website", FieldName = "Website")]
        Website = 3
    }
}