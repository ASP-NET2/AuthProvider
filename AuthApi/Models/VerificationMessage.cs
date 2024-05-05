namespace AuthApi.Models
{
    public class VerificationMessage
    {
        public string Email { get; set; } = null!;
        public string VerificationCode { get; set; } = null!;
    }
}
