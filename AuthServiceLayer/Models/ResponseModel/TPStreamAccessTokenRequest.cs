namespace LMS.API.Models.ResponseModel
{
    public class TPStreamAccessTokenRequest
    {
        public bool expires_after_first_usage { get; set; } = false;
        public List<TPAnnotation> annotations { get; set; }
    }

    public class TPAnnotation
    {
        public string type { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string opacity { get; set; }
        public int size { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int skip { get; set; }
    }

    public class TPStreamAccessTokenResponse
    {
        public string playback_url { get; set; }
        public string code { get; set; }
        public string status { get; set; }
    }

}
