using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.GeoDto
{
    public class GeoUpdateDto
    {
        public float lat { get; set; }

        public float lng { get; set; }
    }
}
