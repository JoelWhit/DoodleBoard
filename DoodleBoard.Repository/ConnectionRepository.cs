using DoodleBoard.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleBoard.Contract.Model;
using DoodleBoard.Model;
using System.Data.Entity;

namespace DoodleBoard.Repository
{
    public class ConnectionRepository : IConnectionRepository
    {
        private DoodleBoardContext _context;
        public ConnectionRepository(DoodleBoardContext context)
        {
            _context = context;
        }

        public IConnection Create(Guid UserId, Guid WhiteboardId)
        {
            return new Connection(UserId, WhiteboardId);
        }

        public void Delete(IConnection entity)
        {
            _context.Connections.Remove((Connection)entity);
        }

        public List<IConnection> GetAll()
        {
            return _context.Connections.ToList<IConnection>();
        }

        public IConnection GetById(int id)
        {
            return _context.Connections.SingleOrDefault(x => x.Id == id);
        }

        public IConnection GetConneciton(Guid Userid, Guid WhiteboardId)
        {
            return _context.Connections.SingleOrDefault(x => x.UserId == Userid && x.WhiteboardId == WhiteboardId);
        }

        public IConnection GetConneciton(IUser User, IWhiteboard Whiteboard)
        {
            return GetConneciton(User.Id, Whiteboard.Id);
        }

        public List<IConnection> GetUserConnections(Guid UserId)
        {
            return _context.Connections.Where(x => x.UserId == UserId).ToList<IConnection>();
        }

        public List<IConnection> GetUserConnections(IUser User)
        {
            return GetUserConnections(User.Id);
        }

        public List<IConnection> GetWhiteboardConnections(Guid WhiteboardId)
        {
            return _context.Connections.Where(x => x.WhiteboardId == WhiteboardId).ToList<IConnection>();
        }

        public List<IConnection> GetWhiteboardConnections(IWhiteboard Whiteboard)
        {
            return GetWhiteboardConnections(Whiteboard.Id);
        }

        public IConnection Save(IConnection entity)
        {
            //_context.Set<User>().Attach(new User(entity.UserId));
            //_context.Set<Whiteboard>().Attach(new Whiteboard(entity.WhiteboardId));

            

            _context.Entry(entity).State = entity.Id == default(int) ?
                             EntityState.Added :
                             EntityState.Modified;

            return entity;

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
