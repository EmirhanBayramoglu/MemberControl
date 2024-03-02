using Dtos.GeoDto;
using MembersControlSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.AddressDto
{
    public class AddressInsertDto
    {
        public string streetName { get; set; }
        public string suiteName { get; set; }
        public string cityName { get; set; }
        public string zipcode { get; set; }
        public GeoInsertDto geoInsertDto { get; set; }
    }
}
