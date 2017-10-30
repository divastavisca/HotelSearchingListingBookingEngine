using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class ResponseGenerationException : Exception
    {
        public override string ToString()
        {
            return "Error in generating response";
        }
    }
}
