using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.Identity.Interfaces;

namespace NoiseCalculator.Infrastructure.Identity.Implementations
{
    public class ExpiryToolIdentityProvider : IIdentityProvider
    {
        private readonly IDirectoryService _directoryService;

        public ExpiryToolIdentityProvider(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }


        public User GetCurrentUser()
        {
            const string shortName = "noreply";
            const string userName = "System";

            var user = new User
                {
                    IsAdmin = false,
                    Shortname = shortName,
                    Fullname = userName
                };

            return user;
        }


        public User GetUserByShortName(string shortName)
        {
            return _directoryService.GetUser(shortName);
        }
    }
}