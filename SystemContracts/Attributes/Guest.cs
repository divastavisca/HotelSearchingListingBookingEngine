using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class Guest
    {
        public Name Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public char Gender { get; set; }

        public string Type { get; set; }
    }
}
