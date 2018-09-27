using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjectDemo.Model.User
{
    public class UserDataStore : IUserDataStore
    {
        private SchoolDBContext _schoolDBContext;

        public UserDataStore(SchoolDBContext schoolDBContext)
        {
            _schoolDBContext = schoolDBContext;
        }

        public IEnumerable<UserDetail> GetAllUsers()
        {
            return _schoolDBContext.Users.ToList();
        }

        public string LogIn(string account, string password, string verificationNumber)
        {
            var users = _schoolDBContext.Users.ToList();
            int selectedUserId = -1;
            foreach (var user in users)
            {
                if(user.Account == account)
                {
                    selectedUserId = user.Id;
                }
                else
                {
                    return "Account not exist";
                }
            }

            UserDetail selectedUser = _schoolDBContext.Users.Find(selectedUserId);
            if (selectedUser == null || selectedUser.Password != password)
            {
                return "user not exist or wrong password";
            }
            else if (selectedUser.Status == 0)
            {
                if(verificationNumber == selectedUser.VerificationNumber)
                {
                    selectedUser.Status = 1;
                    selectedUser.VerificationNumber = null;
                    _schoolDBContext.SaveChanges();
                    return "Activation Successful!";
                }
                else
                {
                    return "wrong verification number";
                }
            }
            else
            {
                //生成token
                var user = _schoolDBContext.Users.Find(selectedUserId);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("ThisissercertKeyforFullStackWebAPIDEMO");
                var tokenDescriptor = new SecurityTokenDescriptor
                 {
                     Subject = new ClaimsIdentity(new Claim[]
                     {
                         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                         new Claim(ClaimTypes.Name, user.Name),
                         new Claim(ClaimTypes.Role, user.UserType)
                     }),
                     Expires = DateTime.UtcNow.AddDays(7),
                     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                 };
                 var token = tokenHandler.CreateToken(tokenDescriptor);
                string stringToken = tokenHandler.WriteToken(token);
                return stringToken + "    is ur token";
            }
        }


        public string SignUp(UserDetail userDetail)
        {
            var users = _schoolDBContext.Users.ToList();
            foreach (var user in users)
            {
                if(user.Account == userDetail.Account.ToLower())
                {
                    return "Error! UserId is existed.";
                }
            }

            if(userDetail.Account.Length < 8 || userDetail.Password.Length < 8)
            {
                return "Error! Makesure userId and password contain at least 8 characters. Can't start with 0";
            }
            else if(userDetail.Phone.Length != 10)
            {
                return "Error! Please enter correct phone number. Your enter "+ userDetail.Phone.ToString();
            }
            string verificationNumber = VerificationNumberGenerator.Generate();
            
            userDetail.Id = 0;
            userDetail.Account = userDetail.Account.ToLower();
            userDetail.Password = userDetail.Password.ToLower();
            userDetail.Status = 0;
            userDetail.VerificationNumber = verificationNumber;
            _schoolDBContext.Users.Add(userDetail);
            _schoolDBContext.SaveChanges();


            try
            {
                var webClient = new WebClient();
                webClient.UseDefaultCredentials = true;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");

                var reqparm = new NameValueCollection();
                reqparm.Add("client_id", "fdzoplaonrDArfYkhS85JVAPnp01wRHJ");
                reqparm.Add("client_secret", "oJIjum7oNWC5E6QC");
                reqparm.Add("grant_type", "client_credentials");

                Byte[] tokenResponse = webClient.UploadValues("https://sapi.telstra.com/v1/oauth/token", reqparm);
                string result = Encoding.UTF8.GetString(tokenResponse);
                var obj = JObject.Parse(result);
                string token = obj.GetValue("access_token").ToString();


                webClient.Headers.Clear();
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                webClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
                webClient.UploadString("https://tapi.telstra.com/v2/messages/provisioning/subscriptions", "{}");

                webClient.Headers.Clear();
                webClient.UseDefaultCredentials = true;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                webClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer "+token);

                PostDataDto postData = new PostDataDto()
                {
                    to = userDetail.Phone,
                    body = "Your Verification Number is " + verificationNumber
                };

                string requestJson = JsonConvert.SerializeObject(postData);
                webClient.UploadString("https://tapi.telstra.com/v2/messages/sms", requestJson);
                return "Success! Please activate your account by sending userId along with Verification Number";

            }
            catch (Exception e)
            {
                return e.Message;
            }


        }

    }
}






//try
//{
//    var webClient = new WebClient();
//    byte[] googleHome = webClient.DownloadData("http://localhost:5000/api/Students");
//    using (var stream = new MemoryStream(googleHome))
//    using (var reader = new StreamReader(stream))
//    {
//        return "chenggong"+reader.ReadToEnd();
//    }

//}
//catch(Exception e)
//{
//    return "shibai"+e.Message;
//}


//try
//{
//    var webClient = new WebClient();
//    webClient.Encoding = Encoding.UTF8;
//    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
//    Student newStudent = new Student() { Id = 0, FullName = "leo"};
//    string json = JsonConvert.SerializeObject(newStudent);
//    string result = webClient.UploadString("http://localhost:5000/api/Students",json);
//     return "chenggong"+result;
//}
//catch(Exception e)
//{
//    return "shibai"+e.Message;
//}





//using (HttpClient client = new HttpClient())
//{
//    try
//    {

//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//    client.DefaultRequestHeaders.Add("Context-Type", "application/json");
//    client.DefaultRequestHeaders.Add("Authorization", "Bearer q3VHPllFh2MWAx2GimPjzpGQ92Xu");

//        PostDataDto postData = new PostDataDto()
//        {
//            To = "0451070619", Priority = false, Validity = "60", Body =("Your verification Number is " + verificationNumber)
//        };

//        String jsonContent = JsonConvert.SerializeObject(postData);
//        HttpContent contentPost = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//    var response = await client.PostAsync("https://tapi.telstra.com/v2/messages/sms", contentPost);
//        string responseString = await response.Content.ReadAsStringAsync;
//        return responseString;
//    }

//    catch(Exception e)
//    {
//        return e.Message;
//    }
//}