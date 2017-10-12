using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class PaymentDetails
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public UserBillingAddress BillingAddress { get; set; }

        public CreditCardDetails CreditCardDetails { get; set; }
    }
}
