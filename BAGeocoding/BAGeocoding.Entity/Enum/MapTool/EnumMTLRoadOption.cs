using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Enum.MapTool
{
    public enum EnumMTLRoadOption : byte
    {
        [EnumItem("Tên đường kiểu số", FieldName = "IsNumber")]
        IsNumber = 0,

        [EnumItem("Đối tượng cầu", FieldName = "IsBridge")]
        IsBridge = 1,

        [EnumItem("Đường nội bộ", FieldName = "IsPrivate")]
        IsPrivate = 2,

        [EnumItem("Đường dành cho động vật", FieldName = "IsPed")]
        IsPed = 3,


        [EnumItem("Có thu phí đường bộ", FieldName = "IsFee")]
        IsFee = 11,


        [EnumItem("Cho phép động vật", FieldName = "AllowPed")]
        AllowPed = 12,

        [EnumItem("Cho phép người đi bộ", FieldName = "AllowWalk")]
        AllowWalk = 13,

        [EnumItem("Xe đạp được phép hoạt động", FieldName = "AllowBicycle")]
        AllowBicycle = 14,

        [EnumItem("Xe máy được phép hoạt động", FieldName = "AllowMoto")]
        AllowMoto = 15,

        [EnumItem("Xe ôtô được phép hoạt động", FieldName = "AllowCar")]
        AllowCar = 16,

        [EnumItem("Xe bus được phép hoạt động", FieldName = "AllowBus")]
        AllowBus = 17,

        [EnumItem("Xe tải được phép hoạt động", FieldName = "AllowTruck")]
        AllowTruck = 18,

        [EnumItem("Xe taxi được phép hoạt động", FieldName = "AllowTaxi")]
        AllowTaxi = 19
    }
}
