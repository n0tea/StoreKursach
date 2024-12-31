using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contract;
using DataAcessLayer.ContextDB;
using DataAcessLayer;
using DataAcessLayer.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class UserService
    {
        private readonly UserDbContext _userContext;
        public UserService(UserDbContext userContext)
        {
            _userContext = userContext;
        }

        public Guid Register(UserCredentials credentials)
        {
            //using var sha = SHA512.Create();
            //var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));  // перенес в отдельный метод

            var user = new User
            {
                Uid = Guid.NewGuid(),
                Email = credentials.Email,
                Password = GetHash(credentials.Password),
                CreationTimestamp = DateTimeOffset.UtcNow,
            };
            _userContext.Set<User>().Add(user);  // можно просто add(user) 
            _userContext.SaveChanges();

            return user.Uid;
        }

        public Guid? Login(UserCredentials credentials)
        {
            var hashed = GetHash(credentials.Password);
            var user = _userContext.Set<User>().SingleOrDefault(x => x.Email == credentials.Email && x.Password == hashed);

            return user?.Uid;
        }

        public UserInfo?/*Contract.UserInfo?*/ GetInfo(Guid uid)
        {
            var user = _userContext.Set<User>().SingleOrDefault(x => x.Uid == uid);

            if (user == null) return null;

            return new Contract.UserInfo
            {
                Uid = user.Uid,
                Email = user.Email,
                //Date = 
            };
        }
       
        private string GetHash(string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes);
        }
    }
}
