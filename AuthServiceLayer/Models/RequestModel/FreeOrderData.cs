namespace LMS.API.Models.RequestModel
{
    public class FreeOrder
    {
        public string order_id { get; set; }
        public int order_amount { get; set; }
        public string order_currency { get; set; }
        public FreeOrderTags order_tags { get; set; }
    }


    public class FreeOrderTags
    {
        public string product_ids { get; set; }
    }

}
