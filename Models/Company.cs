using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MembersControlSystem.Models
{
    public class Company
    {
        [Key]
        [JsonIgnore]
        [Required]
        public int companyId { get; set; }

        [Required]
        public string companyName { get; set; }

        [Required]
        public string catchPhrase { get; set; }

        [Required]
        public string bs { get; set; }

        [JsonIgnore]
        public Member member { get; set; }
    }
}
