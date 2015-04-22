using System.Web.Mvc;
using NoiseCalculator.Infrastructure.Identity.Interfaces;
using NoiseCalculator.UI.Web.Helpers;
using NoiseCalculator.UI.Web.Presenters;

namespace NoiseCalculator.UI.Web.Controllers
{
    public class CurrentUserController : Controller
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly UserPresenter _userPresenter;

        public CurrentUserController(IIdentityProvider identityProvider, UserPresenter userPresenter)
        {
            _identityProvider = identityProvider;
            _userPresenter = userPresenter;
        }


        public ActionResult Execute()
        {
            var user = _identityProvider.GetCurrentUser();
            var userDto = _userPresenter.Present(user);
            return JsonHelper.SerializeObjectToContentResult(userDto);
        }

    }
}