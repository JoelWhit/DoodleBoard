using DoodleBoard.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Contract.Repository
{
    public interface IUserRepository : IBaseRepository<IUser, Guid>
    {
    }
}
