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
        public ActionResult<JwtToken> Register(UserCredentials credentials)
        {
            var uid = _service.Register(credentials);

            return new JwtToken() { Token = _jwtService.GenerateToken(uid, credentials.Email) };
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JwtToken> Login(UserCredentials credentials)
        {
            var uid = _service.Login(credentials);

            if (uid == null)
            {
                ModelState.AddModelError("user", "Invalid email or password");
                return BadRequest(ModelState);
            }
            
            return new JwtToken() { Token = _jwtService.GenerateToken(uid.Value, credentials.Email) };
        }

        [HttpGet]
        [Authorize]
        public ActionResult<UserInfo> GetInfo(Guid uid) {
            
            var user =  _service.GetInfo(uid);
            if (user == null) return NotFound();

            return user;
        }
        /*[HttpDelete]
        public ActionResult Delete(Guid uid) {
            throw new NotImplementedException();
        }*/
    }
}
