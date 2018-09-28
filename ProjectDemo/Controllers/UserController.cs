using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectDemo.Model.User;
using Microsoft.AspNetCore.Authorization;

namespace ProjectDemo.Controllers
{
    [Authorize]
    [Route("api/Users")]
    public class UserController : Controller
    {
        private IUserDataStore _userDataStore;
        public UserController (IUserDataStore userDataStore)
        {
            _userDataStore = userDataStore;
        }
        //Admin权限获取全部学生
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminGet()
        {
            var result = _userDataStore.GetAllUsers();
          return Ok(result);
        }

        //[Authorize(Roles = "User")]
        //[HttpGet("user")]
        //public IActionResult Get()
        //{
        //  var userName = this.User.Identity.Name;
        //    return Ok(userName + "from User");
        //}

      
        [HttpGet]
        public IActionResult GetUserName()
        {
            string stringId = this.User.Identity.Name;
            int id = Convert.ToInt32(stringId);
            string result = _userDataStore.GetUserName(id);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("VerifyEmail/{id}/{verifyCode}")]
        public IActionResult VerifyEmail(int id, string verifyCode)
        {
            var result = _userDataStore.VerifyEmail(id,verifyCode);
            return Ok(result);
        }

        [HttpPost("UpdateEmail")]
        public IActionResult UpdateEmailAddress([FromBody] string email)
        {
            string stringId = this.User.Identity.Name;
            int id = Convert.ToInt32(stringId);
            string result = _userDataStore.UpdateEmailAddress(id,email);
            return Ok(result);
        }


        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            UserDetail userDetail = new UserDetail(){UserType = signUpDto.UserType,Account = signUpDto.Account,Phone = signUpDto.Phone,Password = signUpDto.Password,Name =signUpDto.Name};
            var result = _userDataStore.SignUp(userDetail);

            return Ok(result);
        }
    }
}
