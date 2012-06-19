namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IAdministratorDAO
    {
        bool UserIsAdmin(string username);
    }
}
