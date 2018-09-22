using System;

namespace ProjectDemo.Model.User
{
    public interface IUserDataStore
    {
        string LogIn(string account, string password, string verificationNumber);
        string SignUp(UserDetail userDetail);
    }
}
