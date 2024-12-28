using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contract;
using Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly JwtService _jwtService;
        public UserController(UserService service, JwtService jwtService)
        {
            _service = service;
            _jwtService = jwtService;
        } // в контрукторе

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JwtToken> Register(string email, string password)
        {
            var uid = _service.Register(email, password);

            return new JwtToken() { Token = _jwtService.GenerateToken(uid, email) };
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JwtToken> Login(string email, string password)
        {
            var uid = _service.Login(email, password);

            if (uid == null)
            {
                ModelState.AddModelError("user", "Invalid email or password");
                return BadRequest(ModelState);
            }
            
            return new JwtToken() { Token = _jwtService.GenerateToken(uid.Value, email) };
        }

        [HttpGet]
        [Authorize]
        public ActionResult<UserInfo> GetInfo(Guid uid) {
            throw new NotImplementedException();
        }
        /*[HttpDelete]
        public ActionResult Delete(Guid uid) {
            throw new NotImplementedException();
        }*/
    }
}
