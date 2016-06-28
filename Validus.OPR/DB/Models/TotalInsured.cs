using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class TotalInsured
    {
        [Key]
        public long Id { get; set; }

        public string Class { get; set; }

        public int Total { get; set; }

        public int TotalChecked { get; set; }

        public int TotalErrors { get; set; }

        public int TotalCorrect { get; set; }

        public decimal? SuccessRate { get; set; }

        [JsonIgnore]
        public TotalInsuredReport TotalInsuredReport { get; set; }
    }
}
