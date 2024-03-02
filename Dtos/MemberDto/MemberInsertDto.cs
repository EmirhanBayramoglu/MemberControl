using MembersControlSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos.GeoDto;
using Dtos.AddressDto;
using Dtos.CompanyDto;

namespace Dtos.MemberDto
{
    public class MemberInsertDto
    {
        public string memberName { get; set; }

        public string userName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string memberEmail { get; set; }

        [RegularExpression(@"^[0-9.,\-()]+$", ErrorMessage = "Invalid character.")]
        public string memberPhoneNumber { get; set; }

        public string webSite { get; set; }

        public AddressInsertDto addressInsertDto { get; set; }

        public CompanyInsertDto companyInsertDto { get; set; }

    }
}
