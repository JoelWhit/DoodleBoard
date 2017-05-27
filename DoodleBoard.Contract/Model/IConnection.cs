using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Contract.Model
{
    public interface IConnection
    {
        int Id { get; }
        //IUser User { get; }

        Guid UserId { get; }

        //IWhiteboard Whiteboard { get; }

        Guid WhiteboardId { get; }
    }
}
