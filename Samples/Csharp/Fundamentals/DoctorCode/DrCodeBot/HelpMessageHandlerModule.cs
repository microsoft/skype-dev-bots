using Autofac;
using DrCodeBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;

using System.Diagnostics;

namespace DrCodeBot
{
    public class HelpMessageHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Trace.WriteLine("Load - HelpMessageHandlerModule");
            base.Load(builder);

            builder
                .Register(c => new HelpScorable(c.Resolve<IDialogTask>()))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            Trace.WriteLine("Load - HelpMessageHandlerModule2");
        }
    }
}