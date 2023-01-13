//using System.Web.Services.Protocols;

namespace BAGeocoding.Entity.Utility
{
    public class UTLDataEnum
    {
        public byte IndexID { get; set; }
        public string DataStr { get; set; }

        public UTLDataEnum()
        {
            IndexID = 0;
            DataStr = string.Empty;
        }

        public UTLDataEnum(string indexID, string dataStr)
        {
            IndexID = IndexID;
            DataStr = DataStr;
        }
    }
}
