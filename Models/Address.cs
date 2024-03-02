using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MembersControlSystem.Models
{
    public class Address
    {
        [Key]
        [JsonIgnore]
        [Required]
        public int addressId {  get; set; }
        [Required]
        public string streetName { get; set; }
        [Required]
        public string suiteName { get; set; }
        [Required]
        public string cityName { get; set; }
        [Required]
        public string zipcode { get; set; }

        [Required]
        [JsonIgnore]
        [ForeignKey("geo")]
        public int geoId { get; set; }

        public virtual Geo geo { get; set; }

        [JsonIgnore]
        public Member member { get; set; }
    }
}
