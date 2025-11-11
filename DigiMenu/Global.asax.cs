using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace DigiMenu
{
    public class Global : HttpApplication
    {
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                return;

            FormsAuthenticationTicket ticket;
            try
            {
                ticket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }

            if (ticket == null)
                return;

            // roles no UserData separados por vírgula
            string[] roles = string.IsNullOrEmpty(ticket.UserData)
                ? new string[0]
                : ticket.UserData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Context.User = new GenericPrincipal(new GenericIdentity(ticket.Name, "Forms"), roles);
        }
    }
}
