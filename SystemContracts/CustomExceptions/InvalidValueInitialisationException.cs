using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class InvalidValueInitializationException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Invalid value inizialized in ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}
