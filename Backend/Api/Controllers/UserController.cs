using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contract;
using Backend.Api.Services;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        } // в контрукторе

        [HttpPost]
        public ActionResult<Guid> Register(string email, string password)
        {
            return _service.Register(email, password);
        }

        [HttpPost]
        public ActionResult<Guid> Login(string email, string password)
        {
            var uid = _service.Login(email, password);

            if (uid == null)
            {
                ModelState.AddModelError("user", "Invalid email or password");
                return BadRequest(ModelState);
            }
            return Ok(uid);
        }

        [HttpGet]
        public ActionResult<UserInfo> GetInfo(Guid uid) {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public ActionResult Delete(Guid uid) {
            throw new NotImplementedException();
        }
    }
}
