using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Contract.Service
{
    public interface IWhiteboardService
    {
        bool Join(Guid UserId, Guid WhiteboardId);

        bool WhiteboardExists(Guid WhiteboardId);

    }
}
