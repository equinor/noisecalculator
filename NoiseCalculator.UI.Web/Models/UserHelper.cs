using System.Net.NetworkInformation;

namespace NoiseCalculator.UI.Web.Models
{
    public class UserHelper
    {
        public static string CreateUsernameWithoutDomain(string username)
        {
            if(username.Contains("\\"))
            {
                return username.Split('\\')[1].ToUpper();
            }
            return username;
        }

        public static string CreateDomainUsernameInUppercase(string username)
        {
            return string.Format("{0}\\{1}", IPGlobalProperties.GetIPGlobalProperties().DomainName, username).ToUpper();
        }
    }
}