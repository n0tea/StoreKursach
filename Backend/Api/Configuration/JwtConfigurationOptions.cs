namespace Backend.Api.Configuration
{
    public class JwtConfigurationOptions
    {
        public required string Key { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
    }
}
