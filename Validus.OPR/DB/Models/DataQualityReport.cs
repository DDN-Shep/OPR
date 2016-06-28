using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class DataQualityReport
    {
        [Key]
        public long Id { get; set; }

        public DateTime ReportDate { get; set; }

        public ICollection<DataQuality> DataQualities { get; set; }

        [JsonIgnore]
        public Opr Opr { get; set; }
    }
}
