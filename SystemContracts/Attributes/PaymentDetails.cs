using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class PaymentDetails
    {
        public string Price { get; set; }

        public UserBillingAddress BillingAddress { get; set; }

        public CreditCardDetails CreditCardDetails { get; set; }
    }
}
