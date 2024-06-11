using Logic.Exceptions;
using Logic.Interfaces.Manager;
using Logic.Interfaces.Model;
using Logic.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class UserManager : IUserManager
    {
        IUserRepository userRepository;      
        public UserManager(IUserRepository _userRepository) 
        {
            userRepository = _userRepository;
        }

        public string getSalt(string Username)
        {
            try
            {
                return userRepository.getSalt(Username);
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }

        public IUser Login(IUser user)
        {
            try
            {
                return userRepository.Login(user);
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }
    }
}
