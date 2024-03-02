using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionsClass
{
    public class MatchException : Exception
    {
        public MatchException(string objectType1, string objectType2)
        : base($"Does not match : {objectType1} and {objectType2} ")
        {

        }
    }
}
