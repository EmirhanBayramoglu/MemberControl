using Dtos.GeoDto;
using MembersControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dtos.AddressDto
{
    public class AddressGetDto
    {
        public int addressId { get; set; }

        public string streetName { get; set; }

        public string suiteName { get; set; }

        public string cityName { get; set; }

        public string zipcode { get; set; }

        [JsonIgnore]
        public int geoId { get; set; }

        public virtual Geo geo { get; set; }
    }
}
