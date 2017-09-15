using System.Configuration;
using System.Web;
using System.Web.Http;

namespace Notes
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}