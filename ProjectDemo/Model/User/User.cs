using System;
namespace ProjectDemo.Model.User
{
    public class UserDetail
    {
        public string Name { get; set; }
        public string UserType { get; set; }
        public string Account { get; set; }
        public int Id { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public string VerificationNumber { get; set; }
        public DateTime VNExpiredDate { get; set; }
    }
}
