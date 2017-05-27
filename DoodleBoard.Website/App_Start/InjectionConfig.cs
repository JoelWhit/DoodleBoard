using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using DoodleBoard.Contract.Repository;
using DoodleBoard.Contract.Service;
using DoodleBoard.Repository;
using DoodleBoard.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoodleBoard.Website
{
    public class InjectionConfig
    {
        public static IUnityContainer RegisterInjection()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        public static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IWhiteboardRepository, WhiteboardRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IConnectionRepository, ConnectionRepository>();

            container.RegisterType<IWhiteboardService, WhiteboardService>();
            container.RegisterType<IWhiteboardAuthorizationService, WhiteboardAuthorizationService>();

            return container;
        }
    }
}