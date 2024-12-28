using Microsoft.AspNetCore.Mvc;
using Backend.Api.Services;
using Backend.Api.Contract;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // Добавить новый товар в базу
        [HttpPost]
        [Authorize]
        public ActionResult<Guid> AddProduct([FromBody] AddProductRequest request)
        {
            try
            {
                var productUid = _productService.AddProduct(request);
                return Ok(productUid);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Получить все товары из базы данных
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<ProductResponse>> GetAllProducts()
        {
            try
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Проверить наличие товара по UID
        [HttpGet("{productUid}")]
        [AllowAnonymous]
        public ActionResult<bool> CheckProductAvailability(Guid productUid)
        {
            try
            {
                var isAvailable = _productService.CheckProductAvailability(productUid);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
