﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
{
    public class ServiceProviderGenerationException : Exception
    {
        public override string ToString()
        {
            return "Unable to generate Service Provider to the corresponding request";
        }
    }
}
