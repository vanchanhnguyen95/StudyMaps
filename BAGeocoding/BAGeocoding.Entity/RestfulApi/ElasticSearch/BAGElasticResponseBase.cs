using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Entity.RestfulApi.ElasticSearch
{
    public class BAGElasticResponseBase
    {
        public bool state { get; set; }
        public byte errorcode { get; set; }

        public void InitError(EnumRestfulApiErrorCode errorCode)
        {
            state = false;
            errorcode = (byte)errorCode;
        }
    }
}
