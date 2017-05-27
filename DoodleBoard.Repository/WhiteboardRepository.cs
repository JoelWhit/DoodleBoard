using System;
using System.Collections.Generic;
using System.Linq;
using DoodleBoard.Contract.Repository;
using DoodleBoard.Model;
using DoodleBoard.Contract.Model;
using System.Data.Entity;

namespace DoodleBoard.Repository
{
    public class WhiteboardRepository : IWhiteboardRepository
    {
        private DoodleBoardContext _context;
        public WhiteboardRepository(DoodleBoardContext context)
        {
            _context = context;
        }

        public void Delete(IWhiteboard entity)
        {
            _context.Whiteboards.Remove((Whiteboard)entity);
        }

        public List<IWhiteboard> GetAll()
        {
            return _context.Whiteboards.ToList<IWhiteboard>();
        }

        public IWhiteboard GetById(Guid id)
        {
            return _context.Whiteboards.SingleOrDefault(x => x.Id == id);
        }

        public IWhiteboard Save(IWhiteboard entity)
        {
            _context.Entry(entity).State = entity.Id == default(Guid) ?
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
