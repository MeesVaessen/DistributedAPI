using Logic.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces.Manager
{
    public interface IUserManager
    {
        public IUser Login(IUser user);
        public string getSalt(string Username);
    }
}
