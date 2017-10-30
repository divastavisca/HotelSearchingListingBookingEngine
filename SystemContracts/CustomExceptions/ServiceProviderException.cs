using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class ServiceProviderException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in providing service ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}
