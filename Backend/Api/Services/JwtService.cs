using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contract;
using DataAcessLayer.ContextDB;
using DataAcessLayer;
using DataAcessLayer.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Backend.Api.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Api.Services
{
    public class JwtService
    {
        private readonly IOptions<JwtConfigurationOptions> _options;
        public JwtService(IOptions<JwtConfigurationOptions> options) 
        { 
            _options = options;
        }

        public string GenerateToken(Guid uid, string email)
        {
            //ArgumentNullException.ThrowIfNull(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                //new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, uid.ToString()),
                new Claim(ClaimTypes.Email, email),
                //new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}