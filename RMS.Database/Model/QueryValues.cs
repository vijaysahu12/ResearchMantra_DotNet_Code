using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Database.Model
{
    public class QueryValues
    {
        public int? Id { get; set; }
        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
        public string ThirdKey { get; set; }
        public string FourthKey { get; set; }
        public string FifthKey { get; set; }
        public string SixthKey { get; set; }
        public string SeventhKey { get; set; }
        public long EightKey { get; set; } = 0;

        public int POStatus { get; set; }
        public string SearchText { get; set; }
        public int IsPaging { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortOrder { get; set; }
        public string SortExpression { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int IsAdmin { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }
        public string RoleKey { get; set; }
        public string LoggedInUser { get; set; }
        public string DurationName { get; set; }
        public string PlanName { get; set; }
        public string DeviceVersion { get; set; }
        public int? SubscriptionPlanId { get; set; }
        public int? SubscriptionDurationId { get; set; }
        public int? PostTypeId { get; set; }
        public int? DaysToGo { get; set; }
        public int? ProductId { get; set; }
        public string? ThreeMonthPerformaceChartPeriodType { get; set; } // "months", "quarters", "years"

    }
}
