using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MembersControlSystem.Models
{
    public class Geo
    {
        [Key]
        [JsonIgnore]
        [Required]
        public int geoId { get; set; }
        [Required]
        public float lat { get; set; }
        [Required]
        public float lng { get; set; }

        [JsonIgnore]
        public Address address { get; set; }
    }
}
