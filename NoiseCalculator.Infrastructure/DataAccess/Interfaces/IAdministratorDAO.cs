using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IAdministratorDAO : IDAO<Administrator, string>
    {
        bool UserIsAdmin(string username);
    }
}
