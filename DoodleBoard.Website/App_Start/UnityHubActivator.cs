using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoodleBoard.Website
{
    public class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer _container;

        public UnityHubActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)_container.Resolve(descriptor.HubType);
        }
    }
}