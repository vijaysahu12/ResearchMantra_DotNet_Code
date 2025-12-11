namespace LMS.API.Models.ResponseModel
{
    public class ReviewRequestModel
    {
        public Guid UserPublicKey { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
    }

    public class ReviewRequestUpdateModel
    {
        public string UserPublicKey { get; set; }
        public string ReviewText { get; set; }
    }

}
