using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.CompanyDto
{
    public class CompanyGetDto
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }
    }
}
