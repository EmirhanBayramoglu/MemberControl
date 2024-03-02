using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MembersControlSystem.ExceptionsClass
{
    public class ObjectNotFound : Exception
    {
        public ObjectNotFound(string objectType, string objectId)
        : base($"The specified {objectType} with value {objectId} was not found.")
        {
        }
    }
}

