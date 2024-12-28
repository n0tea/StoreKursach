using Backend.Api.Contract;
using DataAcessLayer.ContextDB;
using DataAcessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services
{
    public class ProductService
    {
        private readonly ProductDbContext _productContext;

        public ProductService(ProductDbContext productContext)
        {
            _productContext = productContext;
        }

        // Добавляем товар в базу данных
        public Guid AddProduct(AddProductRequest request)
        {
            var product = new Product
            {
                Uid = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Quantity = request.Quantity,
                Size = request.Size
            };

            _productContext.Set<Product>().Add(product);
            _productContext.SaveChanges();

            return product.Uid;
        }

        // Возвращаем список всех товаров
        public List<ProductResponse> GetAllProducts()
        {
            return _productContext.Set<Product>()
                .Select(p => new ProductResponse
                {
                    ProductId = p.Uid,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    Size = p.Size
                })
                .ToList();
        }

        // Проверяем, есть ли товар в наличии
        public bool CheckProductAvailability(Guid productUid)
        {
            var product = _productContext.Set<Product>().SingleOrDefault(p => p.Uid == productUid);

            if (product == null) return false;

            return product.Quantity > 0; // Товар доступен, если его количество больше 0
        }
    }
}
