using System;
using System.Collections.Generic;

namespace ProjectDemo.Model.User
{
    public interface IUserDataStore
    {
        string LogIn(string account, string password, string verificationNumber);
        string SignUp(UserDetail userDetail);
        IEnumerable <UserDetail> GetAllUsers();
    }
}
