using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Identity.Interfaces
{
    public interface IIdentityProvider
    {
        User GetCurrentUser();
        User GetUserByShortName(string shortName);
    }
}