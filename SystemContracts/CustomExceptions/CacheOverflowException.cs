using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class CacheOverflowException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in cleaning caches")
                                        .ToString();
        }
    }
}
