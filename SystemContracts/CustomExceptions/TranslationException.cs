using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class TranslationException : Exception
    {
        public override string ToString()
        {
            return "Error in parsing";
        }
    }
}
