using System;
namespace ProjectDemo.Model.User
{
    public class LoginDto
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string VerificationNumber { get; set; }
    }
}
