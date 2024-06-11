using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Interfaces.Model;

namespace Logic.Interfaces.Repository
{
    public interface IUserRepository
    {
        public IUser Login(IUser LogInUser);
        public string getSalt(string Username);

    }
}
