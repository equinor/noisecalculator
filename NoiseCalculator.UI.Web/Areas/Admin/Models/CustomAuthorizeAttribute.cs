using System.Web;
using System.Web.Mvc;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.Support;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{   
    ///marker attribute
    public class CustomAuthorizeAttribute : FilterAttribute { }

    //filter
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IAdministratorDAO _administratorDAO;

        public CustomAuthorizeFilter(IAdministratorDAO administratorDAO)
        {
            _administratorDAO = administratorDAO;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string usernameWithoutDomain = UserHelper.CreateUsernameWithoutDomain(HttpContext.Current.User.Identity.Name);
            if (_administratorDAO.UserIsAdmin(usernameWithoutDomain) == false)
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "AccessDenied" }, { "controller", "Error" } });
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}