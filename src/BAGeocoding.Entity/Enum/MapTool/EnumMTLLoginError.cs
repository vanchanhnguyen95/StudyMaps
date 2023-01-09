using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLLoginError : byte
    {
        [EnumItem("Lỗi phiên bản phần mềm", FieldName = "VersionInvalid")]
        VersionInvalid = 1,

        [EnumItem("Tài khoản bị khóa", FieldName = "AccountBlock")]
        AccountBlock = 2,

        [EnumItem("Không được đăng nhập", FieldName = "AccessDenied")]
        AccessDenied = 3,

        [EnumItem("Không đúng mật khẩu", FieldName = "PasswordWrong")]
        PasswordWrong = 4,

        [EnumItem("Lỗi sai tài khoản", FieldName = "UserNameWrong")]
        UserNameWrong = 5,

        [EnumItem("Không được đăng nhập", FieldName = "MachineIgnore")]
        MachineIgnore = 6,

        [EnumItem("Không có kế hoạch", FieldName = "PlanMission")]
        PlanMission = 7
    }
}
