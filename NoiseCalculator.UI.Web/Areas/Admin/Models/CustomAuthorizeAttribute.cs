using System.Security.Claims;
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
            var username = UserHelper.CreateUsernameWithoutDomain2(HttpContext.Current.User as ClaimsPrincipal);
            
            if (string.IsNullOrEmpty(username))
                username = HttpContext.Current.Session.SessionID;

            string usernameWithoutDomain = UserHelper.CreateUsernameWithoutDomain(username);
            if (_administratorDAO.UserIsAdmin(usernameWithoutDomain) == false)
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "AccessDenied" }, { "controller", "Error" } });
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}