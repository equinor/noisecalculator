using System;
using System.Configuration;
using System.Globalization;
using System.Security.Claims;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace NoiseCalculator.Infrastructure.Identity.Implementations
{
    public class GraphConnectionBuilder
    {
        //private const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
        //private const string LoginUrl = "https://login.windows.net/{0}";
        //private const string GraphUrl = "https://graph.windows.net";
        //private static readonly string GraphApiVersion = ConfigurationManager.AppSettings["ida:GraphApiVersion"];
        //private static readonly string AppPrincipalId = ConfigurationManager.AppSettings["ida:ClientID"];
        //private static readonly string AppKey = ConfigurationManager.AppSettings["ida:Password"];
        
        //public GraphConnection Build()
        //{
        //    // Setup Graph API connection and get a list of users
        //    var clientRequestId = Guid.NewGuid();
        //    var graphSettings = new GraphSettings { ApiVersion = GraphApiVersion };
        //    var accessToken = BuildAuthHeader();
        //    var graphConnection = new GraphConnection(accessToken, clientRequestId, graphSettings);

        //    return graphConnection;
        //}

        //private static string BuildAuthHeader()
        //{
        //    var tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;

        //    // Get a token for calling the Windows Azure Active Directory Graph
        //    var authContext = new AuthenticationContext(String.Format(CultureInfo.InvariantCulture, LoginUrl, tenantId));
        //    var clientCredential = new ClientCredential(AppPrincipalId, AppKey);
        //    var assertionCredential = authContext.AcquireTokenAsync(GraphUrl, clientCredential);
        //    var authHeader = assertionCredential.Result...CreateAuthorizationHeader();

        //    return authHeader;
        //}
    }
}
