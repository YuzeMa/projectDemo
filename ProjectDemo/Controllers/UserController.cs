using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectDemo.Model.User;

// log in and sign up

namespace ProjectDemo.Controllers
{
    [Route("api/Users")]
    public class UserController : Controller
    {
        private IUserDataStore _userDataStore;
        public UserController (IUserDataStore userDataStore)
        {
            _userDataStore = userDataStore;
        }

        [HttpPost("LogIn")]
        public IActionResult LogIn([FromBody] LoginDto loginDto)
        {
            string account = loginDto.Account.ToLower();
            string password = loginDto.Password.ToLower();
            string verificationNumber = loginDto.VerificationNumber;
            var result = _userDataStore.LogIn(account,password,verificationNumber);
            if(result == "user not exist")
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            UserDetail userDetail = new UserDetail(){Account = signUpDto.Account,Phone = signUpDto.Phone,Password = signUpDto.Password,Email =signUpDto.Email};
            var result = _userDataStore.SignUp(userDetail);

            return Ok(result);
        }
    }
}
