using Blog.Domain.Identity;

namespace Blog.Domain.Entities
{
    public class OTPRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string OTP { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; }

        public ApplicationUser User { get; set; }
    }
}