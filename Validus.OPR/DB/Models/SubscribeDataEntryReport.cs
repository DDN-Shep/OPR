using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class SubscribeDataEntryReport
    {
        [Key]
        public long Id { get; set; }

        public DateTime ReportDate { get; set; }

        public ICollection<SubscribeDataEntry> SubscribeDataEntries { get; set; }

        [JsonIgnore]
        public Opr Opr { get; set; }
    }
}
