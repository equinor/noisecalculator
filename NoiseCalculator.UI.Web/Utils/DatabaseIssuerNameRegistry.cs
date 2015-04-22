using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using NoiseCalculator.UI.Web.Models;

namespace NoiseCalculator.UI.Web.Utils
{
    public class DatabaseIssuerNameRegistry : ValidatingIssuerNameRegistry
    {

        private static readonly List<IssuingAuthorityKey> IssuingAuthorityKeys = new List<IssuingAuthorityKey>();
        private static readonly List<Tenant> Tenants = new List<Tenant>();

        public static bool ContainsTenant(string tenantId)
        {
            return Tenants
                .Where(tenant => tenant.Id == tenantId)
                .Any();
        }

        public static bool ContainsKey(string thumbprint)
        {
            return IssuingAuthorityKeys
                .Where(key => key.Id == thumbprint)
                .Any();
        }

        public static void RefreshKeys(string metadataLocation)
        {
            IssuingAuthority issuingAuthority = ValidatingIssuerNameRegistry.GetIssuingAuthority(metadataLocation);

            bool newKeys = false;
            bool refreshTenant = false;
            foreach (string thumbprint in issuingAuthority.Thumbprints)
            {
                if (!ContainsKey(thumbprint))
                {
                    newKeys = true;
                    refreshTenant = true;
                    break;
                }
            }

            foreach (string issuer in issuingAuthority.Issuers)
            {
                if (!ContainsTenant(GetIssuerId(issuer)))
                {
                    refreshTenant = true;
                    break;
                }
            }

            if (!newKeys && !refreshTenant) return;
            if (newKeys)
            {
                //IssuingAuthorityKeys.RemoveRange(context.IssuingAuthorityKeys);
                IssuingAuthorityKeys.Clear();
                foreach (var thumbprint in issuingAuthority.Thumbprints)
                {
                    IssuingAuthorityKeys.Add(new IssuingAuthorityKey {Id = thumbprint});
                }
            }

            foreach (
                string issuerId in
                    issuingAuthority.Issuers.Select(GetIssuerId).Where(issuerId => !ContainsTenant(issuerId)))
            {
                Tenants.Add(new Tenant {Id = issuerId});
            }
        }

        private static string GetIssuerId(string issuer)
        {
            return issuer.TrimEnd('/').Split('/').Last();
        }

        protected override bool IsThumbprintValid(string thumbprint, string issuer)
        {
            return ContainsTenant(GetIssuerId(issuer))
                   && ContainsKey(thumbprint);
        }



        //    private static readonly List<IssuingAuthorityKey> IssuingAuthorityKeys = new List<IssuingAuthorityKey>();
        //    private static readonly List<Tenant> Tenants = new List<Tenant>();


        //    public static bool ContainsTenant(string tenantId)
        //    {
        //        return Tenants.Any(tenant => tenant.Id == tenantId);
        //    }

        //    public static bool ContainsKey(string thumbprint)
        //    {
        //        return IssuingAuthorityKeys.Any(key => key.Id == thumbprint);
        //    }

        //    public static void RefreshKeys(string metadataLocation)
        //    {
        //        var issuingAuthority = GetIssuingAuthority(metadataLocation);

        //        var newKeys = false;
        //        var refreshTenant = false;
        //        if (issuingAuthority.Thumbprints.Any(thumbprint => !ContainsKey(thumbprint)))
        //        {
        //            newKeys = true;
        //            refreshTenant = true;
        //        }

        //        if (issuingAuthority.Issuers.Any(issuer => !ContainsTenant(GetIssuerId(issuer))))
        //        {
        //            refreshTenant = true;
        //        }

        //        if (!newKeys && !refreshTenant) return;
        //        if (newKeys)
        //        {
        //            //IssuingAuthorityKeys.RemoveRange(context.IssuingAuthorityKeys);
        //            IssuingAuthorityKeys.Clear();
        //            foreach (var thumbprint in issuingAuthority.Thumbprints)
        //            {
        //                IssuingAuthorityKeys.Add(new IssuingAuthorityKey { Id = thumbprint });
        //            }
        //        }

        //        foreach (string issuerId in issuingAuthority.Issuers.Select(GetIssuerId).Where(issuerId => !ContainsTenant(issuerId)))
        //        {
        //            Tenants.Add(new Tenant { Id = issuerId });
        //        }
        //    }

        //    private static string GetIssuerId(string issuer)
        //    {
        //        return issuer.TrimEnd('/').Split('/').Last();
        //    }

        //    protected override bool IsThumbprintValid(string thumbprint, string issuer)
        //    {
        //        return ContainsTenant(GetIssuerId(issuer))
        //            && ContainsKey(thumbprint);
        //    }
        //}
    }
}

