using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Reputation { get; set; }

        public User(int userId, string username, string password, DateTime registationTime, string email ,int reputation)
        {
            UserId = userId;
            RegistrationTime = registationTime;
            UserName = username;
            Email = email;
            Password = password;
            Reputation = reputation;
        }
    }
}
