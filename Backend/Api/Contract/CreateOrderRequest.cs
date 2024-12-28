namespace Backend.Api.Contract
{
    public class CreateOrderRequest
    {
        public List<OrderProduct> Products { get; set; } = new List<OrderProduct>();
    }

    public class OrderProduct
    {
        public Guid ProductId { get; set; } // Уникальный ID товара
        public int Quantity { get; set; }   // Количество
    }
}
