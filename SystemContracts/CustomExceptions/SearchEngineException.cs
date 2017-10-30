using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class SearchEngineException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in processing request error occured at ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}
