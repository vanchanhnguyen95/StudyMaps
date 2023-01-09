using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMCLUserDataExt : byte
    {
        [EnumItem("Đăng nhập hệ thống", FieldName = "LoginWin")]
        LoginWin = 0,

        [EnumItem("Đăng nhập app mobile", FieldName = "LoginMbl")]
        LoginMbl = 1,

        [EnumItem("Đăng nhập website", FieldName = "LoginWeb")]
        LoginWeb = 2
    }
}