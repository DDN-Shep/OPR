using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class SubscribeDataEntry
    {
        [Key]
        public long Id { get; set; }

        public string BusinessPlan { get; set; }

        public int CompletedOnTime { get; set; }

        public int CompletedLate { get; set; }

        public decimal? CompletedSuccessRate { get; set; }

        public int OutstandingOnTime { get; set; }

        public int OutstandingLate { get; set; }

        public decimal? OutstandingSuccessRate { get; set; }

        [JsonIgnore]
        public SubscribeDataEntryReport SubscribeDataEntryReport { get; set; }
    }
}
