using System.Net.NetworkInformation;
using System.Security.Claims;

namespace NoiseCalculator.UI.Web.Support
{
    public class UserHelper
    {
        public static string CreateUsernameWithoutDomain(string username)
        {
            var shortName = username;

            if(!string.IsNullOrEmpty(username) && username.Contains("\\"))
            {
                shortName = username.Split('\\')[1].ToUpper();
            }

            if (!string.IsNullOrEmpty(shortName) && shortName.Contains("@"))
                return shortName.Substring(0, shortName.IndexOf("@", System.StringComparison.Ordinal));

            return shortName;
        }

        public static string CreateUsernameWithoutDomain2(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsPrincipal = user;
            var claimName = claimsPrincipal.FindFirst(ClaimTypes.Name);

            string shortName = null;
            var username = claimName.Value;

            if (!string.IsNullOrEmpty(username) && username.Contains("\\"))
                shortName = username.Split('\\')[1].ToUpper();

            if (!string.IsNullOrEmpty(shortName) && shortName.Contains("@"))
                return shortName.Substring(0, shortName.IndexOf("@", System.StringComparison.Ordinal));

            return shortName;
        }

        public static string CreateDomainUsernameInUppercase(string username)
        {
            return string.Format("{0}\\{1}", IPGlobalProperties.GetIPGlobalProperties().DomainName, username).ToUpper();
        }
    }
}