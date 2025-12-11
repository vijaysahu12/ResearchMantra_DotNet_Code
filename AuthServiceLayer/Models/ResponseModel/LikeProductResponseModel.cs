namespace LMS.API.Models.ResponseModel
{
    public class LikeProductRequestModel
    {
        
            public int ProductId { get; set; }
            public int LikeId { get; set; }
            public string CreatedBy { get; set; }
            public string Action { get; set; }
        }
    
}
