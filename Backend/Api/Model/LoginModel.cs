using Logic.Interfaces.Model;

namespace Api.Model
{
    public class LoginModel : IUser
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public int ID { get; set; }
    }
}
