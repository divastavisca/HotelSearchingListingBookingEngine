using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class FactoryException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Unable to create instance requested type ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}
