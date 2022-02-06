using InterviewTask.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InterviewTask
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            ILoggerService log = new LoggerService();
            log.LogError("Unhandled error:", "Global", exception);
            Server.ClearError();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var logger = new LoggerService();

            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            string url = HttpContext.Current.Request.ServerVariables["HTTP_URL"];

            var sb = new StringBuilder();
            sb.Append("Page Requested:");
            sb.Append(DateTime.Now.ToString());
            sb.Append(" IP:");
            sb.Append(ip);
            sb.Append(" URL:");
            sb.Append(url);
            logger.LogTrace(sb.ToString());\
        }


    }
}
