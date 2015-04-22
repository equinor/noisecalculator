using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Identity.Interfaces
{
	public interface IDirectoryService
	{
        User GetUser(string userName);
        IList<User> FindUsers(string query, int limit);
	}	
}
