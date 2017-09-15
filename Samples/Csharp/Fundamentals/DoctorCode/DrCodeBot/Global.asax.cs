using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace DrCodeBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.RegisterBotModules();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void RegisterBotModules()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ReflectionSurrogateModule());
            builder.RegisterModule<HelpMessageHandlerModule>();
            builder.Update(Conversation.Container);
        }
    }
}
