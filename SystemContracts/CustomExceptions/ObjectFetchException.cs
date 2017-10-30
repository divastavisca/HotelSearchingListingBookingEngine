using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class ObjectFetchException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Unable to fetch object ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}
