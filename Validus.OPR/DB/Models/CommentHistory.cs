using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class CommentHistory
    {
        [Key]
        public long Id { get; set; }

        public ICollection<Comments> Comments { get; set; }

        [JsonIgnore]
        public Opr Opr { get; set; }
    }
}
