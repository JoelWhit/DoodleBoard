using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Contract.Service
{
    public interface IWhiteboardAuthorizationService
    {
        bool IsAuthorized(Guid UserId, Guid WhiteboardId);
    }
}
