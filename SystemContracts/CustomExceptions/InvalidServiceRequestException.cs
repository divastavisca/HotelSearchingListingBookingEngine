using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class InvalidServiceRequestException : Exception
    {
        public override string ToString()
        {
            return "Invalid service requested by origin";
        }
    }
}
