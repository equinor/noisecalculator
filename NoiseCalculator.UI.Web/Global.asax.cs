using System;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoiseCalculator.UI.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Task", action = "Index", id = UrlParameter.Optional}, // Parameter defaults
                new[]{ "NoiseCalculator.UI.Web.Controllers" }
                );

        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["init"] = 0;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // ....

            string configPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Web.config";
            string metadataAddress =
                            ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            ValidatingIssuerNameRegistry.WriteToConfig(metadataAddress, configPath); 

            // Make sure we don't use the Webforms view engine in any way.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(Object Sender, EventArgs e)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

            if (routeData != null && string.IsNullOrEmpty(routeData.GetRequiredString("controller")) == false)
            {
                string cultureName = null;

                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null)
                    cultureName = cultureCookie.Value;
                else
                    cultureName = "nb-NO";
                //cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // User needs to be redirected back to the originally requested page.
            // WSFAM will only parse the WSFed message and redirect back to the page there, 
            // which is Login/?ReturnUrl=<OriginallyRequestedPage>.
            // So the following code handles the last redirection step.
            // This needs to be in EndRequest, because WsFAM calls Application.CompleteRequest after doing its redirect.
            string wsFamRedirectLocation = HttpContext.Current.Response.RedirectLocation;
            if (wsFamRedirectLocation != null && wsFamRedirectLocation.Contains("ReturnUrl") && User.Identity.IsAuthenticated)
            {
                HttpContext.Current.Response.RedirectLocation =
                    HttpUtility.ParseQueryString(wsFamRedirectLocation.Split('?')[1])["ReturnUrl"];
            }
        }
    }
}