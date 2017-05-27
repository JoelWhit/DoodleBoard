using System;

namespace DoodleBoard.Contract.Model
{
    public interface IWhiteboard 
    {
        Guid Id { get; }

        bool HasPassword { get; }

        string Password { get; }

        DateTime Created { get; }


    }
}
