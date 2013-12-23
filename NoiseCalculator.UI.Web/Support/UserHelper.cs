using System.Net.NetworkInformation;
using System.Security.Claims;

namespace NoiseCalculator.UI.Web.Support
{
    public class UserHelper
    {
        public static string CreateUsernameWithoutDomain(string username)
        {
            string shortName = username;

            if(!string.IsNullOrEmpty(username) && username.Contains("\\"))
            {
                shortName = username.Split('\\')[1].ToUpper();
            }

            if (!string.IsNullOrEmpty(username) && username.Contains("@"))
                return shortName.Substring(0, shortName.IndexOf("@", System.StringComparison.Ordinal));

            return username;
        }

        public static string CreateUsernameWithoutDomain2(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsPrincipal = user;
            var claimName = claimsPrincipal.FindFirst(ClaimTypes.Email);
            return claimName != null ? claimName.Value : "Name is not set";
        }

        public static string CreateDomainUsernameInUppercase(string username)
        {
            return string.Format("{0}\\{1}", IPGlobalProperties.GetIPGlobalProperties().DomainName, username).ToUpper();
        }
    }
}