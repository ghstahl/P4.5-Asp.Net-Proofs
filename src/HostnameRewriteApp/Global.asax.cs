using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;

namespace HostnameRewriteApp
{


    public class HostNameRewriteMiddleware : OwinMiddleware
    {
        public HostNameRewriteMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Headers.ContainsKey("x-hostname-rewrite"))
            {
                var hostName = context.Request.Headers["x-hostname-rewrite"];
                context.Request.Host = new HostString(hostName);
            }
              
            await Next.Invoke(context);
          
        }
    }
    public class LoggingMiddleware : OwinMiddleware
    {
        public LoggingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            Console.WriteLine("Begin Request");
            await Next.Invoke(context);
            Console.WriteLine("End Request");
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(typeof(LoggingMiddleware));
            app.Use(typeof(HostNameRewriteMiddleware));

        }
    }
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
