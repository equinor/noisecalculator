using NoiseCalculator.Domain.Entities;
using NoiseCalculator.UI.Web.DTOs;

namespace NoiseCalculator.UI.Web.Presenters
{
    public class UserPresenter
    {
        public UserDTO Present(User user)
        {
            var userDto = new UserDTO()
            {
                Shortname = user.Shortname.ToLower(),
                Fullname = user.Fullname,
                Email = user.Email.ToLower(),
                IsAdmin = user.IsAdmin
            };

            return userDto;
        }
    }
}