using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        public string GetUserName(int id)
        {
            UserDetail loginUser = _schoolDBContext.Users.Find(id);
            return loginUser.Name;
        }

        public string VerifyEmail(int id, string verifyCode)
        {
            UserDetail selectedUser =_schoolDBContext.Users.Find(id);
            if(selectedUser.EVExpiredDate < DateTime.Now)
            {
                selectedUser.Email = null;
                selectedUser.Email_Verification = null;
                _schoolDBContext.SaveChanges();
                return "Your Verification Link is Expired! Please Update Email Again! ";
            }
            else
            {
                if(selectedUser.Email_Verification == verifyCode)
                {
                    selectedUser.Email_Status = 1;
                    selectedUser.Email_Verification = null;
                    _schoolDBContext.SaveChanges();
                    return "Success, Your Email is Activated";
                }
                else
                {
                    return "Error!";
                }
            }
        }

        public string UpdateEmailAddress(int id, string email)
        {
            if(IsMailFormat(email))
            {
                UserDetail loginUser = _schoolDBContext.Users.Find(id);
                loginUser.Email = email;
                loginUser.Email_Status = 0;
                string verificationCode = MD5(VerificationNumberGenerator.Generate());
                loginUser.Email_Verification = verificationCode;
                loginUser.EVExpiredDate = DateTime.Now.AddDays(1.0);
                _schoolDBContext.SaveChanges();
                string verifyURL = string.Format("http://localhost:5000/api/Users/VerifyEmail/{0}/{1}",id, verificationCode);
                Mailbox(email, verifyURL);
                return "Please Check Ur Email " + _schoolDBContext.Users.Find(id).Email;
            }
            else
            {
                return "please enter valid email address";
            }
        }

        private static string MD5(string code)
        {

            string MD5code = ""; 

            byte[] buffer = Encoding.UTF8.GetBytes(code);   //convert to Byte[]    

            byte[] MD5buffer = new MD5CryptoServiceProvider().ComputeHash(buffer); //instantiation and encryption

            foreach (byte item in MD5buffer)
                //convert each byte[] to string, from duotricemary（32） to Hexadecimal（16)
            {
                MD5code += item.ToString("X2");                 
            }
            return MD5code;
        }

        private static bool Mailbox(string clientEmail, string verifyURL)
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("tuscmedia.au@gmail.com");

            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   // You can try with 465 also
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; 
            smtp.UseDefaultCredentials = false; 
            smtp.Credentials = new NetworkCredential("tuscmedia.au@gmail.com", "Ma950522");  // account,psw
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress(clientEmail));
            mail.Subject = "mailbox verification";
            //Formatted mail body
            mail.IsBodyHtml = true;

            mail.Body = "click this link to verify ur Email : " + verifyURL;
            smtp.Send(mail);
            return true;
        }

        //checking email format
        private static bool IsMailFormat(string mailFormat)
        {
            for (int i = 0; i < mailFormat.Length; ++i)
            {
                if (';' == mailFormat[i])
                {
                    return false;
                }
            }

            return Regex.IsMatch(mailFormat, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
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
            var salt = selectedUser.Salt;
            var inputPassword = PSWEncryption(password, salt);
            if (selectedUser == null || selectedUser.Password != inputPassword)
            {
                return "user not exist or wrong password";
            }
            else if (selectedUser.Phone_Status == 0)
            {
                DateTime timeNow = DateTime.Now;
                if (timeNow > selectedUser.VNExpiredDate)
                {
                    _schoolDBContext.Users.Remove(selectedUser);
                    _schoolDBContext.SaveChanges();
                    return "Expired, Please Sign Up Again!";
                }

                if(verificationNumber == selectedUser.VerificationNumber)
                {
                    selectedUser.Phone_Status = 1;
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
                         //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                         //new Claim(ClaimTypes.Name, user.Name),
                         new Claim(ClaimTypes.Name, user.Id.ToString()),
                         new Claim(ClaimTypes.Role, user.UserType)
                     }),
                    Expires = DateTime.UtcNow.AddHours(10.0),
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
            DateTime expiredDateTime = DateTime.Now.AddMinutes(10.0);
            bool result = SendMessage(userDetail.Phone, verificationNumber, expiredDateTime);
            if(result==false)
            {
                return "Error! while sending message! ";
            }
            userDetail.Id = 0;
            userDetail.Account = userDetail.Account.ToLower();
            string salt = GetSalt();
            userDetail.Salt = salt;
            userDetail.Password = PSWEncryption(userDetail.Password.ToLower(), salt);
            userDetail.Phone_Status = 0;
            userDetail.VerificationNumber = verificationNumber;
            userDetail.VNExpiredDate = expiredDateTime;
            _schoolDBContext.Users.Add(userDetail);
            _schoolDBContext.SaveChanges();
            return "Success! Please activate your account by sending userId along with Verification Number";

        }


        private string GetSalt ()
        {
            // random salt 
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider(); 
            byte[] saltBytes = new byte[36]; 
            rng.GetNonZeroBytes(saltBytes); 
            string salt = Convert.ToBase64String(saltBytes);
            return salt;
        }

        private string PSWEncryption(string psw, string salt)

        {

            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(psw + salt);
            byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);

            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        private static bool SendMessage(string phone, string verificationNumber, DateTime expiredDateTime)
        {
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
                webClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);

                PostDataDto postData = new PostDataDto()
                {
                    to = phone,
                    body = "Your Verification Number is " + verificationNumber + "\r\n Expired By " + expiredDateTime.ToString()
                };

                string requestJson = JsonConvert.SerializeObject(postData);
                webClient.UploadString("https://tapi.telstra.com/v2/messages/sms", requestJson);
                return true;
            }
            catch
            {
                return false;
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
