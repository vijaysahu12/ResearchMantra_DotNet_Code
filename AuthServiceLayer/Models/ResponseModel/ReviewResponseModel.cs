namespace LMS.API.Models.ResponseModel
{
    public class ReviewResponseModel
    {
        public int Id { get; set; }
        public Guid UserPublicKey { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public bool IsAdminApproved { get; set; }
    }

}
