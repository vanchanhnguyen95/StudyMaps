using System;
using System.Data;

using BAGeocoding.Entity.Enum.MapObject;
using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    /// <summary>
    /// Thông tin bảng
    /// </summary>
    public class CTLObjectTableName : CatalogBase
    {
        #region ==================== Contructor Init ===============================
        public short TableID { get; set; }
        public EnumMOBObjectTable EnumTableID { get { return (EnumMOBObjectTable)TableID; } set { TableID = (short)value; } }
        public string Name { get; set; }
        #endregion

        #region ==================== Get From DataRow ==============================
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                if (base.FromDataRow(dr) == false)
                    return false;

                TableID = base.GetDataValue<short>(dr, "TableID");
                Name = base.GetDataValue<string>(dr, "Name");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("CTLObjectTableName.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }
        #endregion
    }
}