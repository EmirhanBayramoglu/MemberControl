using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.RegistrationDto
{
    public class UserForRegistration
    {

        [Required(ErrorMessage = "UserName is required.")]
        [ForeignKey("Members")]
        public string? userName { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        public ICollection<string>? Roles { get; init; }
    }
}
