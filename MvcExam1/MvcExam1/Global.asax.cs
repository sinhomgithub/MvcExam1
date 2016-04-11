using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MvcExam1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            // 解開登入 Cookie 還原 Role 資料, 存到 User.Identity
            if (User != null && 
                User.Identity != null && 
                User.Identity.IsAuthenticated && 
                User.Identity is FormsIdentity)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = (id.Ticket);
                if (!string.IsNullOrEmpty(ticket.UserData))
                {
                    string userData = ticket.UserData;
                    string[] roles = userData.Split(',');

                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
                }
            }
        }

    }
}
