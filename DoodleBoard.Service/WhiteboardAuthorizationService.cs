using DoodleBoard.Contract.Repository;
using DoodleBoard.Contract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Service
{
    public class WhiteboardAuthorizationService : IWhiteboardAuthorizationService
    {
        private readonly IConnectionRepository _repoitory;
        public WhiteboardAuthorizationService(IConnectionRepository repository)
        {
            _repoitory = repository;
        }
        public bool IsAuthorized(Guid UserId, Guid WhiteboardId)
        {
            return (_repoitory.GetConneciton(UserId, WhiteboardId) != null);
        }
    }
}
