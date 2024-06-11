using Logic.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Models
{
    public class User : IUser
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
