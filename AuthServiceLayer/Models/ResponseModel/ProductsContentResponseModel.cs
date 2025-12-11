using KRCRM.Database.KingResearchContext;

namespace LMS.API.Models.ResponseModel
{

    public class ProductContentResponse
    {
        public List<ChapterResponseModel> Chapters { get; set; }
    }

    public class LMSFlatContentModel
    {
        public int ChapterId { get; set; }
        public string? ChapterTitle { get; set; }
        public string? ChapterDescription { get; set; }

        public int? SubChapterId { get; set; }
        public string? SubChapterTitle { get; set; }
        public string? SubChapterLink { get; set; }
        public string? SubChapterDescription { get; set; }
        public string? SubChapterLanguage { get; set; }
        public int? VideoDuration { get; set; }
        public bool? IsVisible { get; set; }
        public string? AttachmentType { get; set; }
        public string? LMSVideoLink { get; set; }
        public string? LMSVideoId { get; set; }
        public int? LMSVideoDuration { get; set; }

    }


    public class LMSChapterResponseModel
    {
        public int Id { get; set; }
        public string ChapterTitle { get; set; }
        public string? Description { get; set; }
        public List<LMSSubChapterResponseModel> SubChapters { get; set; }
    }


    public class LMSSubChapterResponseModel
    {
        
            public int? Id { get; set; }
            public string? Title { get; set; }
            public string? Link { get; set; }
            public string? LMSVideoLink { get; set; }
            public string? Description { get; set; }
            public string? Language { get; set; }
            public int? VideoDuration { get; set; }
            public int? LMSVideoDuration { get; set; }
            public bool? IsVisible { get; set; }
            public string? AttachmentType { get; set; }
        
    }


}
