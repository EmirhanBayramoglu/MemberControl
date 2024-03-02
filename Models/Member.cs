using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembersControlSystem.Models
{
    public class Member
    {
        [Key]
        [Required]
        public int memberId { get; set; }

        [Required]
        public string memberName { get; set; }

        [Required]
        public string userName { get; set; }

        [Required]
        [MinLength(6,ErrorMessage ="Password must be at least 6 character")]
        public string password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string memberEmail { get; set; }

        [Required]
        [RegularExpression(@"^[0-9.,\-()]+$", ErrorMessage ="Invalid character.")]
        public string memberPhoneNumber { get; set; }

        [Required]
        public string webSite { get; set; }

        [ForeignKey("Addresses")]
        public int addressId { get; set; }
        public virtual Address address { get; set; }

        [ForeignKey("Companies")]
        public int companyId { get; set; }
        public virtual Company company { get; set; }

        public Member()
        {
            password = "mahmut123";
        }
    }
}