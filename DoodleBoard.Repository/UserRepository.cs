using DoodleBoard.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleBoard.Contract.Model;
using System.Data.Entity;
using DoodleBoard.Model;

namespace DoodleBoard.Repository
{
    public class UserRepository : IUserRepository
    {
        private DoodleBoardContext _context;
        public UserRepository(DoodleBoardContext context)
        {
            _context = context;
        }

        public void Delete(IUser entity)
        {
            _context.Users.Remove((User)entity);
        }

        public List<IUser> GetAll()
        {
            return _context.Users.ToList<IUser>();
        }

        public IUser GetById(Guid id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id);
        }

        public IUser Save(IUser entity)
        {
            //_context.Entry(entity).State = entity.Id == default(Guid) ?
            //     EntityState.Added :
            //     EntityState.Modified;
            //_context.Users.Attach((User)entity);
            _context.Users.Add((User)entity);
            //_context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
