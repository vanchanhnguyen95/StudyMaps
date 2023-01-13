using System.Collections;

namespace BAGeocoding.Entity.DataService
{
    public class DTSRConfig
    {
        public Hashtable Authen { get; set; }
        public Hashtable Register { get; set; }

        public DTSRConfig()
        {
            Authen = new Hashtable();
            Register = new Hashtable();
        }
    }
}
