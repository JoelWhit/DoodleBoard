using System;
using System.Threading.Tasks;
using DoodleBoard.Contract.Service;
using Microsoft.AspNet.SignalR;

namespace DoodleBoard.Website.Hubs
{

    public class DoodleBoardHub : Hub
    {         
        private readonly IWhiteboardService _whiteboardService;
        private readonly IWhiteboardAuthorizationService _authService;

        public DoodleBoardHub(IWhiteboardService wbService, IWhiteboardAuthorizationService authService)
        {
            _whiteboardService = wbService;
            _authService = authService;
        }

        //[SignalRAuthorize]
        public void Send(string whiteboardId, string coordinates)
        {
            // could possibly cache this locally in a concurrect dictionary or in the service
            if (_authService.IsAuthorized(Guid.Parse(Context.ConnectionId), Guid.Parse(whiteboardId)))
            {
#if DEBUG
                Clients.Group(whiteboardId).recieveCoordinates(coordinates);
#else          
                Clients.OthersInGroup(whiteboardId).recieveCoordinates(coordinates);
#endif
            }
        }

        public void Join(Guid whiteboardId)
        {
            if (_whiteboardService.Join(Guid.Parse(Context.ConnectionId), whiteboardId))
            {
                Groups.Add(Context.ConnectionId, whiteboardId.ToString());
            }
        }

        /*
        * https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections#groups
        * You should not manually remove the user from the group when the user disconnects. This action is automatically performed by the SignalR framework. 
        */
        public void Leave(string whiteboardId)
        {
            Groups.Remove(Context.ConnectionId, whiteboardId);
        }
    }
}