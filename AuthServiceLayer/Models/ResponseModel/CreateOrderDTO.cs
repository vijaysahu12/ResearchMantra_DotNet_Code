namespace LMS.API.Models.ResponseModel
{
    public class CreateOrderDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
        public List<int> ProductIds { get; set; }  
    }

}
