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
                // Получаем пользовательские данные (идентификатор и email) из токена
                var (userId, email) = GetUserInfoFromToken();
                if (userId == null || email == null)
                {
                    return Unauthorized();
                }

                // Создаем заказ для пользователя
                var orderUid = _orderService.CreateOrder(userId.Value, email, request);

                return Ok(orderUid);
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, возвращаем BadRequest с подробностями
                return BadRequest(new { Error = ex.Message });
            }
        }

        private (long? userId, string? email) GetUserInfoFromToken()
        {
            // Извлекаем Uid пользователя из токена
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null)
                return (null, null);

            if (!Guid.TryParse(userClaim.Value, out var userUid))
                return (null, null);

            // Извлекаем почту из токена
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            var email = emailClaim?.Value;

            var userId = _orderService.GetUserIdByUid(userUid);

            return (userId, email);
        }
    }
}
