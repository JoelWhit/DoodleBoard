using DoodleBoard.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Contract.Repository
{
    public interface IConnectionRepository : IBaseRepository<IConnection, int>
    {
        IConnection Create(Guid UserId, Guid WhiteboardId);

        IConnection GetConneciton(IUser User, IWhiteboard Whiteboard);
        IConnection GetConneciton(Guid Userid, Guid WhiteboardId);

        List<IConnection> GetUserConnections(IUser User);
        List<IConnection> GetUserConnections(Guid UserId);

        List<IConnection> GetWhiteboardConnections(IWhiteboard Whiteboard);
        List<IConnection> GetWhiteboardConnections(Guid WhiteboardId);






    }
}
