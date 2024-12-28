namespace Backend.Api.Contract
{
    public class AddProductRequest
    {
        public string Name { get; set; } // Название товара
        public string Description { get; set; } // Описание
        public int Quantity { get; set; } // Количество
        public string Size { get; set; } // Размер (например, L, XL)
    }
}
