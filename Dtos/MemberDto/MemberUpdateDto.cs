using Dtos.AddressDto;
using Dtos.CompanyDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.MemberDto
{
    public class MemberUpdateDto
    {

        public string memberName { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 character")]
        public string password { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string memberEmail { get; set; }

        [RegularExpression(@"^[0-9.,\-()]+$", ErrorMessage = "Invalid character.")]
        public string memberPhoneNumber { get; set; }

        public string webSite { get; set; }

        /*public AddressUpdateDto addressUdpdateDto { get; set; }

        public CompanyUpdateDto companyUpdateDto { get; set; }*/
    }
}
