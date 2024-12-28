using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Contract;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Guid> Register(string email, string password)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult<Guid> Login(string email, string password)
        {
            throw new NotImplementedException();
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
