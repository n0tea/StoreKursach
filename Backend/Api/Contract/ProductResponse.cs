namespace Backend.Api.Contract
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
}
