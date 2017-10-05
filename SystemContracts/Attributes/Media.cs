using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class Media
    {
        public string Type { get; }

        public string Name { get; }

        public string Url { get; }

        public Media(string type,string name,string url)
        {
            Type = type;
            Name = name;
            Url = url;
        }
    }
}
