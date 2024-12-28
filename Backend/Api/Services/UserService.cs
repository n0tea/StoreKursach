using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contract;
using DataAcessLayer.ContextDB;
using DataAcessLayer;
using DataAcessLayer.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Api.Services
{
    public class UserService
    {
        private readonly UserDbContext _userContext;
        public UserService(UserDbContext userContext)
        {
            _userContext = userContext;
        }

        public Guid Register(string email, string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            var user = new User
            {
                Uid = Guid.NewGuid(),
                Email = email,
                Password = GetHash(password),
                CreationTimestamp = DateTimeOffset.UtcNow,
            };
            _userContext.Set<User>().Add(user);
            _userContext.SaveChanges();

            return user.Uid;
        }

        public Guid? Login(string email, string password)
        {
            var hashed = GetHash(password);
            var user = _userContext.Set<User>().SingleOrDefault(x => x.Email == email && x.Password == hashed);

            return user?.Uid;
        }

        public Contract.UserInfo? GetInfo(Guid uid)
        {
            var user = _userContext.Set<User>().SingleOrDefault(x => x.Uid == uid);

            if (user == null) return null;

            return new Contract.UserInfo
            {
                Uid = user.Uid,
                Email = user.Email,
            };
        }

        public bool Delete(Guid uid)
        {
            throw new NotImplementedException();
        }

        private string GetHash(string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes);
        }
    }
}
