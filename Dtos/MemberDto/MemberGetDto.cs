using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MembersControlSystem.Models;
using Dtos.AddressDto;
using Dtos.CompanyDto;
using System.Text.Json.Serialization;

namespace Dtos.MemberDto
{
    public class MemberGetDto
    {
        public int memberId { get; set; }

        public string memberName { get; set; }

        public string userName { get; set; }

        public string memberEmail { get; set; }

        public string memberPhoneNumber { get; set; }

        public string webSite { get; set; }

        [JsonIgnore]
        public int addressId { get; set; }

        [JsonIgnore]
        public int CompanyId { get; set; }

        public virtual Address address { get; set; }

        public virtual Company company { get; set; }
    }
}
