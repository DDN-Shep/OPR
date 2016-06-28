using Microsoft.AspNet.SignalR;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace Validus.OPR
{
    public static class HubExtension
    {
        public static IIdentity GetUserIdentity(this Hub hub)
        {
            var user = hub.Context.User;

            return user != null
                && user.Identity != null
                && user.Identity.IsAuthenticated
                 ? user.Identity
                 : null;
        }
        public static string GetUserName(this Hub hub)
        {
            var identity = hub.GetUserIdentity();

            return identity != null
                 ? identity.Name
                 : null;
        }
        public static string GetUserDisplayName(this Hub hub)
        {
            var name = hub.GetUserName();

            if (name != null)
            {
                var user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain), name);

                if (user != null) return user.DisplayName;
            }

            return null;
        }
    }
}