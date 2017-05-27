using DoodleBoard.Contract.Model;
using System;
using System.Collections.Generic;

namespace DoodleBoard.Model
{
    public class Whiteboard : IWhiteboard
    {
        public Whiteboard(string password)
        {
            Password = password;
        }

        public Whiteboard(Guid ID)
        {
            Id = ID;
        }

        private Whiteboard() //EF constructor
        {

        }

        public DateTime Created { get; private set; } = DateTime.UtcNow;

        public bool HasPassword
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Password);
            }
        }

        public Guid Id { get;  set; }

        public string Password { get; private set; }



        public virtual ICollection<Connection> Connections { get; set; }

        
    }
}
