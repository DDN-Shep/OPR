using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class TotalInsuredReport
    {
        [Key]
        public long Id { get; set; }

        public DateTime ReportDate { get; set; }

        public ICollection<TotalInsured> TotalInsureds { get; set; }

        [JsonIgnore]
        public Opr Opr { get; set; }
    }
}
