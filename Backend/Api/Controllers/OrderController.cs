using Microsoft.AspNetCore.Mvc;
using Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend.Api.Contract;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Guid> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                // Получаем Uid текущего пользователя из токена
                var userUid = GetUserUidFromToken();
                if (userUid == null) return Unauthorized();

                // Создаем заказ для пользователя
                var orderUid = _orderService.CreateOrder(userUid.Value, request);

                return Ok(orderUid);
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, возвращаем BadRequest с подробностями
                return BadRequest(new { Error = ex.Message });
            }
        }

        private Guid? GetUserUidFromToken()
        {
            // Извлекаем Uid пользователя из токена
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Убедитесь, что Uid пользователя включается в токен
            if (userClaim == null) return null;

            return Guid.TryParse(userClaim.Value, out var userUid) ? userUid : null;
        }
    }
}
