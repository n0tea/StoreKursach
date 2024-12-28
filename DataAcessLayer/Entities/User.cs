namespace DataAcessLayer.Entities
{
    public class User
    {
        public long Id { get; init; }
        public Guid Uid { get; init; } //= Guid.NewGuid();
        public string Email { get; set; }
        public string Password { get; set; }
        public int? Height { get; set; }
        public decimal? Weight { get; set; }
        public int? ChestCircumference { get; set; }
        public int? WaistCircumference { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
    }
}
