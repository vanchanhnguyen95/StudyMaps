using BAGeocoding.Entity.Enum;
using BAGeocoding.Entity.Enum.MapTool;

namespace BAGeocoding.Entity.MapTool.Plan
{
    public class WRKPlanOptionExport
    {
        public EnumMTLExportDataState EnumState { get; set; }
        public EnumBAGObjectType EnumTypeID { get; set; }
        public string FileName { get; set; }
        public string GridStr { get; set; }
        public byte PKind { get; set; }

        public bool IgnoreBlankData { get; set; }
        public bool IgnoreDeleteByCut { get; set; }
        public bool IgnoreDeleteByJoin { get; set; }
    }
}
