using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using DoodleBoard.Contract.Service;

namespace DoodleBoard.Website.Hubs
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SignalRAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly IWhiteboardAuthorizationService _service;
        private string _userId;
        private string _whiteboardId;

        public SignalRAuthorizeAttribute()
        {
            _service = (IWhiteboardAuthorizationService)GlobalHost.DependencyResolver.GetService(typeof(IWhiteboardAuthorizationService));
        }
            


        protected override bool UserAuthorized(System.Security.Principal.IPrincipal user)
        {
            _userId = HttpContext.Current.Request.RequestContext.RouteData.Values["UserId"].ToString();
            _whiteboardId = HttpContext.Current.Request.RequestContext.RouteData.Values["WhiteboardId"].ToString();

            return _service.IsAuthorized(Guid.Parse(_userId), Guid.Parse(_whiteboardId));
        }
    }
}