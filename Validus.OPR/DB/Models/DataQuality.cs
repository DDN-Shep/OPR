using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class DataQuality
    {
        [Key]
        public long Id { get; set; }

        public string BusinessPlan { get; set; }

        public int TotalSlips { get; set; }

        public int SlipsChecked { get; set; }

        public decimal? SlipPassRate { get; set; }

        public decimal? SoxPassRate { get; set; }

        public decimal? FieldErrorRate { get; set; }

        public decimal? SoxFieldErrorRate { get; set; }

        [JsonIgnore]
        public DataQualityReport DataQualityReport { get; set; }
    }
}
