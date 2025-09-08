namespace RestautantMvc.DTOs
{
    public class LoginResponse
    {
        public string?  Token { get; set; }
        public string? Username { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
