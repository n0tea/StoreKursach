using Confluent.Kafka;
using Backend.Api.Contract;
using DataAcessLayer.ContextDB;
using DataAcessLayer.Entities;

namespace Backend.Api.Services
{
    public class OrderService
    {
        private readonly OrderDbContext _orderContext;
        private readonly ProductDbContext _productContext;
        private readonly IProducer<string, string> _kafkaProducer;

        public OrderService(OrderDbContext orderContext, ProductDbContext productContext, IProducer<string, string> kafkaProducer)
        {
            _orderContext = orderContext;
            _productContext = productContext;
            _kafkaProducer = kafkaProducer;
        }

        public Guid CreateOrder(Guid userUid, CreateOrderRequest request)
        {
            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var product in request.Products)
            {
                var dbProduct = _productContext.Set<Product>().SingleOrDefault(p => p.Uid == product.ProductId);
                if (dbProduct == null || dbProduct.Quantity < product.Quantity)
                    throw new Exception($"Not enough stock for product with ID {product.ProductId}");

                var itemPrice = product.Quantity * dbProduct.Quantity;

                var orderItem = new OrderItem
                {
                    Uid = Guid.NewGuid(),
                    //OrderId = Guid.Empty, // Temporary, update later
                    ProductId = dbProduct.Id,
                    Quantity = product.Quantity,
                    Price = itemPrice
                };
                orderItems.Add(orderItem);

                dbProduct.Quantity -= product.Quantity;
                totalPrice += itemPrice;
            }

            _productContext.SaveChanges();

            var order = new Order
            {
                Uid = Guid.NewGuid(),
                UserId = userUid,
                TotalPrice = totalPrice,
                CreationTimestamp = DateTimeOffset.UtcNow
            };

            _orderContext.Set<Order>().Add(order);
            _orderContext.SaveChanges();

            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                _orderContext.Set<OrderItem>().Add(item);
            }
            _orderContext.SaveChanges();

            // Отправляем сообщение в Kafka
            SendOrderToKafka(order.Uid, userUid, totalPrice);

            return order.Uid;
        }

        private void SendOrderToKafka(Guid orderUid, Guid userUid, decimal totalPrice)
        {
            var message = new
            {
                OrderId = orderUid,
                UserId = userUid,
                TotalPrice = totalPrice,
                Timestamp = DateTimeOffset.UtcNow
            };

            _kafkaProducer.Produce("order", new Message<string, string>
            {
                Key = orderUid.ToString(),
                Value = System.Text.Json.JsonSerializer.Serialize(message)
            });

            _kafkaProducer.Flush(TimeSpan.FromSeconds(5)); // Обеспечиваем отправку
        }
    }
}
