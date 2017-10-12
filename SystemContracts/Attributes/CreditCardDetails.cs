using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class CreditCardDetails
    {
       public string NameOnCard { get; set; } 

       public string CardNumber { get; set; }

       public string Cvv { get; set; }

       public string Code { get; set; }

       public string CardName { get; set; }

       public DateTime Expiry { get; set; }

       public bool IsThreeDAuth { get; set; }
    }
}
