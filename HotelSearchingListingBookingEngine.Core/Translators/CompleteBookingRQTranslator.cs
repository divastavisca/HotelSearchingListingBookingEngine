using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class CompleteBookingRQTranslator
    {
        public CompleteBookingRQ Translate(ProductStagingInfo productStagingInfo)
        {
            try
            {
                return new CompleteBookingRQ()
                {
                    TripFolderId = productStagingInfo.TripFolderId,
                    ExternalPayment = getPaymentDetails(productStagingInfo),
                    SessionId = productStagingInfo.CallerSessionId
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = baseException.Source
                };
            }
        }

        private CreditCardPayment getPaymentDetails(ProductStagingInfo cardPayment)
        {
            CreditCardPayment payment = new CreditCardPayment();
            CreditCardPayment paymentPlaceHolder = (CreditCardPayment)cardPayment.Payment;
            payment.Amount = cardPayment.ProductFare;
            payment.BillingAddress = cardPayment.Payment.BillingAddress;
            payment.CardMake = paymentPlaceHolder.CardMake;
            payment.CardType = paymentPlaceHolder.CardType;
            payment.ExpiryMonthYear = paymentPlaceHolder.ExpiryMonthYear;
            payment.NameOnCard = paymentPlaceHolder.NameOnCard;
            payment.IsThreeDAuthorizeRequired = payment.IsThreeDAuthorizeRequired;
            payment.Number = payment.Number;
            payment.Attributes = new StateBag[]
                   {
                           new StateBag() { Name="API_SESSION_ID", Value=cardPayment.CallerSessionId},
                           new StateBag(){ Name="PointOfSaleRule"},
                           new StateBag(){ Name="SectorRule"},
                           new StateBag(){ Name="_AttributeRule_Rovia_Username"},
                           new StateBag(){ Name="_AttributeRule_Rovia_Password"},
                           new StateBag(){ Name="AmountToAuthorize",Value=cardPayment.Payment.Amount.ToString()},
                           new StateBag(){ Name="PaymentStatus",Value="Authorization successful"},
                           new StateBag(){ Name="AuthorizationTransactionId",Value=Guid.NewGuid().ToString()},
                           new StateBag(){ Name="ProviderAuthorizationTransactionId",Value=Guid.NewGuid().ToString()},
                           new StateBag(){ Name="PointOfSaleRule"},
                           new StateBag(){ Name="SectorRule"},
                           new StateBag(){ Name="_AttributeRule_Rovia_Username"},
                           new StateBag(){ Name="_AttributeRule_Rovia_Password"}
                   };
            return payment;
        }
    }
}
