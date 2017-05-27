using DoodleBoard.Contract.Model;
using DoodleBoard.Contract.Repository;
using DoodleBoard.Contract.Service;
using DoodleBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Service
{
    public class WhiteboardService : IWhiteboardService
    {
        private readonly IConnectionRepository _cnRepository;
        private readonly IWhiteboardRepository _wbRepository;
        private readonly IUserRepository _urRepository;

        public WhiteboardService(IConnectionRepository cnRepository, IWhiteboardRepository wbRepository, IUserRepository urRepository)
        {
            _cnRepository = cnRepository;
            _wbRepository = wbRepository;
            _urRepository = urRepository;
        }


        public bool Join(Guid UserId, Guid WhiteboardId)
        {
            IWhiteboard wb = _wbRepository.GetById(WhiteboardId);
            if (wb == null) { return false; };

            IUser ur = CreateUserIfNotExists(UserId);

            IConnection con = _cnRepository.Create(ur.Id, wb.Id);
            _cnRepository.Save(con);
            _cnRepository.SaveChanges();

            return true;
        }   

        public bool WhiteboardExists(Guid WhiteboardId)
        {
            return (_wbRepository.GetById(WhiteboardId) != null);
        }

        private IUser CreateUserIfNotExists(Guid user)
        {
            IUser usr = _urRepository.GetById(user);

            if (usr == null)
            {
                usr = new User(user);
                _urRepository.Save(usr);
                _urRepository.SaveChanges();
            }

            return usr;
        }
    }
}
