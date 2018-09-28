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
        public string Salt { get; set; }
        public string Email { get; set; }
        public int Email_Status { get; set; }
        public string Email_Verification { get; set; }
        public DateTime EVExpiredDate { get; set; }
        public string Phone { get; set; }
        public int Phone_Status { get; set; }
        public string VerificationNumber { get; set; }
        public DateTime VNExpiredDate { get; set; }
    }
}
