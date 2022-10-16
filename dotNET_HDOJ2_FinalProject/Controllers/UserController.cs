using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace dotNET_HDOJ2_FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private compilerContext _CompilerContext = new compilerContext();


        //[DllImport("D:/HDOJ_dotNET/dotNET_HDOJ2_FinalProject/x64/Debug/EncryptionService.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Add")]
        [DllImport(@"..\HDOJ_dotNET\DLL\AES.dll", EntryPoint = "Encrypt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
        public static extern int Encrypt(string str, ref byte res);
        [DllImport(@"..\HDOJ_dotNET\DLL\AES.dll", EntryPoint = "GetMD5", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
        public static extern int GetMD5(string str, ref byte res);
        [DllImport(@"..\HDOJ_dotNET\DLL\AES.dll", EntryPoint = "Decrypt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
        public static extern int Decrypt(string str, ref byte res);

        //public static extern int Add(int a, int b);

        public static string AesEncrypt(string str)
        {
            byte[] s = new byte[1024];
            int l = Encrypt(str, ref s[0]);
            byte[] data = null;
            if (l > s.Length) return "";
            else
            {
                data = new byte[l];
                Buffer.BlockCopy(s, 0, data, 0, l);
                string strGet = System.Text.Encoding.Default.GetString(data, 0, data.Length);
                return strGet;
            }
        }

        public static string MD5Encrypt(string str)
        {
            byte[] s = new byte[1024];
            int l = GetMD5(str, ref s[0]);
            byte[] data = null;
            if (l > s.Length) return "";
            else
            {
                data = new byte[l];
                Buffer.BlockCopy(s, 0, data, 0, l);
                string strGet = System.Text.Encoding.Default.GetString(data, 0, data.Length);
                return strGet;
            }
        }
        public static string CreateData(string msg, string encrypt)
        {
            string buffer = "";
            string ms = msg + encrypt;
            int length = ms.Length;
            int cou = 8 - length.ToString().Length;
            for (int i = 0; i < cou; i++)
            {
                buffer += "0";
            }
            buffer += length.ToString() + ms;
            return buffer;
        }
        public static bool DataDecrypt(string msg)
        {
            int a = 0; int index = 0;
            for (int i = msg.Length - 1; i >= 0; i--)
            {
                a++;
                if (a == 96)
                {
                    index = i;
                    break;
                }
            }
            string aes = ""; string data = "";
            aes = msg.Substring(index);
            data = msg.Substring(0, msg.Length - 96);
            string md5 = MD5Encrypt(data);
            byte[] s = new byte[1024];
            int l = Decrypt(aes, ref s[0]);
            byte[] redata = null; string strGet = "";
            if (l > s.Length) strGet = "";
            else
            {
                redata = new byte[l];
                Buffer.BlockCopy(s, 0, redata, 0, l);
                strGet = System.Text.Encoding.Default.GetString(redata, 0, redata.Length);
            }
            if (md5 == strGet) return true;
            return false;
        }

        [HttpPost("login")]
        public async Task<JObject> Login([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string Username = jsonParams.username;
            string Password = jsonParams.password;

            string securedPassword = CreateData(MD5Encrypt(Password), AesEncrypt(Password));

            JObject returnJson = new JObject();

            IQueryable<User> TmpUser = from tmpuser in _CompilerContext.Users where tmpuser.Username == Username select tmpuser;

            User user = TmpUser.FirstOrDefault();

            Console.WriteLine(user.Username + " " + user.Password);

            if (user == null)
            {
                returnJson.Add("code", 400);
                returnJson.Add("message", "用户不存在");
            }
            else if (user.Password != securedPassword)
            {
                returnJson.Add("code", 400);
                returnJson.Add("message", "用户不存在");
            }
            else
            {
                returnJson.Add("code", 200);
                returnJson.Add("message", "登陆成功");

                returnJson.Add("id", user.UserId);
                returnJson.Add("username", user.Username);
                returnJson.Add("avatar", user.Avatar);
                returnJson.Add("email", user.Email);
                returnJson.Add("type", user.Type);
            }

            return returnJson;
        }

        [HttpPost("register")]
        public async Task<JObject> Register([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string Username = jsonParams.username;
            string Email = jsonParams.email;
            string Password = jsonParams.password;
            string University = jsonParams.university;
            int Age = jsonParams.age;

            string securedPassword = CreateData(MD5Encrypt(Password), AesEncrypt(Password));
            //Console.WriteLine("res: " + c);

            JObject returnJson = new JObject();

            IQueryable<User> TmpUser = from tmpuser in _CompilerContext.Users where tmpuser.Username == Username select tmpuser;
            User user = TmpUser.FirstOrDefault();

            if (user == null)
            {
                User addUser = new User();
                addUser.Username = Username;
                addUser.Email = Email;
                addUser.Password = securedPassword;
                addUser.University = University;
                addUser.Age = Age;
                addUser.Type = 1;

                _CompilerContext.Users.Add(addUser);
                _CompilerContext.SaveChanges();

                returnJson.Add("code", 200);
                returnJson.Add("message", "注册成功");

                IQueryable<User> TmpUser1 = from tmpuser1 in _CompilerContext.Users where tmpuser1.Username == Username select tmpuser1;

                User user1 = TmpUser1.FirstOrDefault();

                returnJson.Add("id", user1.UserId);
                returnJson.Add("username", user1.Username);
                returnJson.Add("avatar", user1.Avatar);
                returnJson.Add("email", user1.Email);
                returnJson.Add("type", user1.Type);


            }
            else
            {
                returnJson.Add("code", 400);
                returnJson.Add("message", "注册失败");
            }

            return returnJson;
        }

        [HttpGet("getUserInfoUnique")]
        public async Task<String> getUserInfo([FromQuery] long userId)
        {
            IQueryable<User> Usernamequry =
                from user in _CompilerContext.Users where user.UserId == userId select user;
            User usernamequry = Usernamequry.FirstOrDefault();
            return usernamequry.Username.ToString();
        }
    }
}
