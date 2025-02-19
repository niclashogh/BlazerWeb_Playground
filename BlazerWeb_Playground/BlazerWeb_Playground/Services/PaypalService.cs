using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Exceptions;
using System.Diagnostics;

namespace BlazerWeb_Playground.Services
{
    public class PaypalService
    {
        private PaypalServerSdkClient client;
        private OrdersController orderController;
        private PaypalWalletCustomer customer;

        private string oAuthId = "AZS2C2THIQTVFN61Tsi9Y1JJRItVRwPYqpzD6gLdNq5hybhg63_x7sDizgkI4wBddqu2wAQbWTI05GeF";
        private string oAuthSecret = "EH3ruXFrh9-97J2z4FwiUwNkv3YnoDyxT09kQZa2Hqh6xHHU2NAfZk4YNczubhOJZxE0N9xiK1-v8S69";

        public PaypalService()
        {
            client = new PaypalServerSdkClient.Builder()
                .ClientCredentialsAuth(new ClientCredentialsAuthModel.Builder(oAuthId, oAuthSecret).Build())
                .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
                .LoggingConfig(config => config.LogLevel(LogLevel.Information)
                    .RequestConfig(config => config.Body(true))
                    .ResponseConfig(config => config.Body(true)))
                .Build();

            orderController = client.OrdersController;
        }

        public async void CreateOrder()
        {
            OrdersCreateInput order = new OrdersCreateInput
            {
                Body = new OrderRequest
                {
                    Intent = CheckoutPaymentIntent.Capture,

                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            Amount = new AmountWithBreakdown
                            {
                                CurrencyCode = "currency_code6",
                                MValue = "value0"
                            }
                        }
                    }//,

                    //Payer = new Payer
                    //{
                    //    EmailAddress = "",
                    //    PayerId = "",
                    //    Phone = new PhoneWithType(new PhoneNumber(""), PhoneType.Mobile)
                    //}
                },

                Prefer = "return=representation"
            };

            try
            {
                ApiResponse<Order> result = await orderController.OrdersCreateAsync(order);
                Debug.WriteLine("Result, Header:" + result.Headers);
                Debug.WriteLine("Result, Data: " + result.Data);
                Debug.WriteLine("Result, StatusCode: " + result.StatusCode);
            }
            catch (ApiException e)
            {

            }
        }

        public OrdersConfirmInput ConfirmOrderPayment()
        {
            return new OrdersConfirmInput
            {
                Id = "",
                Prefer = "return=presentation",

                Body = new ConfirmOrderRequest
                {
                    PaymentSource = new()
                }
            };
        }

        public OrdersAuthorizeInput AuthorizaOrder()
        {
            return new OrdersAuthorizeInput
            {
                Id = "",
                Prefer = "return=minimal"
            };
        }
    }
}
