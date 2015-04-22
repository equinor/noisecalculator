using System.Security.Claims;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.Identity.Interfaces;

namespace NoiseCalculator.Infrastructure.Identity.Implementations
{
    public class ClaimsIdentityProvider : IIdentityProvider
    {
        private readonly IDirectoryService _directoryService;
        //private readonly IAdminByShortnameQuery _adminByShortnameQuery;

        public ClaimsIdentityProvider(IDirectoryService directoryService) //,
            //IAdminByShortnameQuery adminByShortnameQuery)
        {
            _directoryService = directoryService;
            //_adminByShortnameQuery = adminByShortnameQuery;
        }


        public User GetCurrentUser()
        {
            var userPrincipalName = ClaimsPrincipal.Current.Identity.Name;
            var user = _directoryService.GetUser(userPrincipalName);
            user.IsAdmin = IsUserAdmin(userPrincipalName);
            return user;
        }


        public User GetUserByShortName(string shortname)
        {
            var userPrincipalName = string.Format("{0}@statoil.com", shortname);
            var user = _directoryService.GetUser(userPrincipalName);
            user.IsAdmin = IsUserAdmin(userPrincipalName);
            return user;
        }

        private bool IsUserAdmin(string userPrincipalName)
        {
            var shortname = userPrincipalName.Split('@')[0].ToLower();
            //var admin = _adminByShortnameQuery.Execute(shortname);
            //return (admin != null);
            return false;
        }
    }
}