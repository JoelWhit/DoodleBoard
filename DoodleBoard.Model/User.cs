using DoodleBoard.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleBoard.Model
{
    public class User : IUser
    {
        public User(Guid ID)
        {
            Id = ID;
        }

        private User() //EF Constructor
        {

        }
        public Guid Id { get; private set; }

        public virtual ICollection<Connection> Connections { get; set; }
    }
}
