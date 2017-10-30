using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class ObjectInitializationException : Exception
    {
        

        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in initilizing ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}
