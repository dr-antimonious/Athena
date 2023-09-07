namespace Artemis.Contracts.DTOs
{
    public class TokenDto
    {
        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}
