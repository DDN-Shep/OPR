using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Validus.OPR.DB.Models
{
    public class Comments
    {
        [Key]
        public long Id { get; set; }

        public string Type { get; set; }

        public string Comment { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        
        [JsonIgnore]
        public CommentHistory CommentHistory { get; set; }
    }
}