using DoodleBoard.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Model
{
    public class Connection : IConnection
    {
        public int Id { get; private set; }
        public Connection(Guid userId, Guid whiteboardId)
        {
            WhiteboardId = whiteboardId;
            UserId = userId;
        }

        private Connection() //ef constructor
        {

        }


        public Guid WhiteboardId { get; private set; }
        public Guid UserId { get; private set; }


        //navigation properties
        public virtual User User { get; set; }
        public virtual Whiteboard Whiteboard { get; set; }

        
    }
}
