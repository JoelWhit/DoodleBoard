using System;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(DoodleBoard.Website.Startup))]

namespace DoodleBoard.Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // signalR configuration
            GlobalHost.DependencyResolver = new UnitySignalRDependencyResolver(InjectionConfig.BuildUnityContainer());
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(InjectionConfig.BuildUnityContainer()));

            // backplanes
            //https://www.nuget.org/packages/Microsoft.AspNet.SignalR.ServiceBus/
            //https://www.nuget.org/packages/Microsoft.AspNet.SignalR.Redis/

            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(30);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(10);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(GlobalHost.Configuration.DisconnectTimeout.Seconds / 3);

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}