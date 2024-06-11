using Dal.Interfaces;
using Dal.Models;
using Google.Protobuf;
using Logic.Exceptions;
using Logic.Interfaces.Model;
using Logic.Interfaces.Repository;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDBContext _context;

        public UserRepository(IDBContext context)
        {
            _context = context;
        }

        public string getSalt(string Username)
        {
            User user;
            try
            {
                user = _context.Users
                .Where(u => u.Name == Username)
                .First();

                if (user == null)
                {
                    throw new NotFoundException();

                }
                return user.Salt;
            }
            catch (MySqlException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        public IUser Login(IUser LogInUser)
        {
            User user;
            try
            {
                user = _context.Users
                .Where(u => u.Name == LogInUser.Name)
                .First();

                if (user == null || user.Password != LogInUser.Password)
                {
                    throw new NotFoundException();
                    
                }
                return user;
            }
            catch (MySqlException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

    }
}
