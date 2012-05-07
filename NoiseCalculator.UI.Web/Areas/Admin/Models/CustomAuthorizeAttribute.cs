using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    ///marker attribute
    public class CustomAuthorizeAttribute : FilterAttribute { }

    //filter
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private string[] users = new[] { "LAKHA", "HLF", @"STATOIL-NET\lakha" };

        public CustomAuthorizeFilter(IDAO<NoiseProtection, int> noiseProtectionDAO)
        {
            string lol = "lol";
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (users.Contains(HttpContext.Current.User.Identity.Name))
            {
                string hasUser = "hasUser";
            }
            else
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "AccessDenied" }, { "controller", "Error" } });
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}