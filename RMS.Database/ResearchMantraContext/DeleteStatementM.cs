using System;

namespace KRCRM.Database.KingResearchContext
{
    public class DeleteStatementM
    {
        public int Id { get; set; }
        public string Statement { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDelete { get; set; }
    }
}
