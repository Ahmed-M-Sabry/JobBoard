namespace JobBoard.Infrastructure.Authentication
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public DateTime? ExpiryAt { get; set; }
        public bool IsExpired => ExpiryAt <= DateTime.UtcNow;

        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
