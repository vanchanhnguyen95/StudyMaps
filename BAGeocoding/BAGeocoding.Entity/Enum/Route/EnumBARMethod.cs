namespace BAGeocoding.Entity.Enum.Route
{
    public enum EnumBARMethod : byte
    {
        // Tìm đường nhanh một cặp (trả về khoảng cách)
        SRoute = 9,
        // Tìm đường nhanh nhiều cặp (trả về khoảng cách)
        SRouteExt = 10,

        // Tìm đường đầy đủ một cặp (trả về chi tiết)
        FRoute = 17,
        // Tìm đường đầy đủ nhiều cặp (trả về chi tiết)
        FRouteExt = 18
    }
}
