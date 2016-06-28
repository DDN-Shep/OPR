using System;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class Opr
    {
        [Key]
        public long Id { get; set; }

        public DataQualityReport DataQualityReport { get; set; }

        public TotalInsuredReport TotalInsuredReport { get; set; }

        public SubscribeDataEntryReport SubscribeDataEntryReport { get; set; }

        public CommentHistory CommentHistory { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}