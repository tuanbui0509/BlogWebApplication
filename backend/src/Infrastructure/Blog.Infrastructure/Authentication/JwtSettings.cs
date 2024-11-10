namespace Blog.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}