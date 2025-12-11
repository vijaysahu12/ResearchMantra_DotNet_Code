using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext.LMS
{
    public class AddToCart
    {
        public int Id { get; set; }                          
        public Guid UserPublicKey { get; set; }              
        public int ProductMId { get; set; }                   
        public bool IsActive { get; set; } = true;          
        public bool IsDelete { get; set; } = false;         
        public bool IsPurchased { get; set; } = false;         
        public string? CreatedBy { get; set; }              
        public DateTime CreatedDate { get; set; }            
        public string? ModifiedBy { get; set; }              
        public DateTime? ModifiedDate { get; set; }
    }
}
