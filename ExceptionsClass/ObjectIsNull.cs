using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembersControlSystem.ExceptionsClass
{
    public class ObjectIsNull : Exception
    {
        public ObjectIsNull(string objectType)
        : base($"This object is null: {objectType}.")
        {
        }
    }
}
