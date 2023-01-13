namespace BAGeocoding.Entity.Enum.Route
{
    public enum EnumBARType : byte
    {
        // Tìm đường ngắn nhất (Theo khoảng cách)
        Normal = 0,
        // Tìm đường nhanh nhất (Có hệ số hiệu chỉnh)
        Adjust = 1
    }
}
