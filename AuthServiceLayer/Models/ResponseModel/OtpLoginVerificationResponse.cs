namespace AuthServiceLayer.Models.ResponseModel
{
    public class OtpLoginVerificationResponse
    {
        public string? Result { get; set; }
        public Guid MobileUserKey { get; set; }
        public long MobileUserId { get; set; }
        public string MobileNumber { get; set; }
        public string? EmailTemp { get; set; }
        public string? FullName { get; set; }
        public string? EventSubscription { get; set; }
        public string? Message { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImage { get; set; }
        public bool? IsExistingUser { get; set; }
        public bool IsFreeTrialActivated { get; set; }
        public string? LeadKey { get; set; }
        public int FreeTrialDays { get; set; }
        public string productNames { get; set; }
        public string WelcomeMessage { get; set; }
    }
}
