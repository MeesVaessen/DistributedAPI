using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces.Model
{
    public interface IUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
