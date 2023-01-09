using System.Web.Services.Protocols;

namespace BAGeocoding.Entity.Utility
{
    public class UTLAuthHeader : SoapHeader
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UTLAuthHeader()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public UTLAuthHeader(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public bool Equal(UTLAuthHeader other)
        {
            if (Username != other.Username)
                return false;
            else if (Password != other.Password)
                return false;
            else
                return true;
        }

        public override string ToString()
        {
            return string.Format("Username: '{0}', Password: '{1}'", Username, Password);
        }
    }
}
