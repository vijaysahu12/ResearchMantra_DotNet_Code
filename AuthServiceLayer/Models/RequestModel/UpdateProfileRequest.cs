namespace LMS.API.Models.RequestModel
{
    public class UpdateProfileRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Gender { get; set; } = "M";
        public DateTime DateOfBirth { get; set; }
        public string Language { get; set; } = "English";
        public string About { get; set; }
    }
}
