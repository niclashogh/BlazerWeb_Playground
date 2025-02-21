using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Exceptions;

namespace BlazerWeb_Playground.Services
{
    // Sandbox: https://developer.paypal.com/dashboard/applications/sandbox
    // Toturial at: https://developer.paypal.com/serversdk/http/getting-started/how-to-get-started/

    public class PaypalService
    {
        #region Variables & Properties
        private PaypalServerSdkClient client;
        private OrdersController orderController;
        public PaypalWalletCustomer? Customer {  get; private set; }
        public Order? Order { get; private set; }
        public string? OrderApproveUrl
        {
            get
            {
                if (Order != null && Order.Links != null)
                {
                    return Order.Links.FirstOrDefault(lnk => lnk.Rel == "approve")?.Href;
                }
                else return null;
            }
        }

        private string oAuthId = "AZS2C2THIQTVFN61Tsi9Y1JJRItVRwPYqpzD6gLdNq5hybhg63_x7sDizgkI4wBddqu2wAQbWTI05GeF";
        private string oAuthSecret = "EH3ruXFrh9-97J2z4FwiUwNkv3YnoDyxT09kQZa2Hqh6xHHU2NAfZk4YNczubhOJZxE0N9xiK1-v8S69";
        #endregion

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

        public async Task<bool> CreateOrder(double totalPrice)
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
                                CurrencyCode = "USD",
                                MValue = totalPrice.ToString()
                            }
                        }
                    },

                    ApplicationContext = new OrderApplicationContext
                    {
                        ReturnUrl = "https://localhost:7142/paypal-receipt",
                        CancelUrl = "https://localhost:7142/paypal-receipt"
                    }
                },

                Prefer = "return=representation"
            };

            try
            {
                ApiResponse<Order> response = await orderController.OrdersCreateAsync(order);
                this.Order = response.Data;
                ConsoleLog(response, "CreateOrder");
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine($"\nPaypalService.CreateOrder ERROR:\n{e}\n");
                return false;
            }
        }

        public async Task<bool> CaptureOrder(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    OrdersCaptureInput capture = new OrdersCaptureInput
                    {
                        Id = orderId,
                        Prefer = "return=representation"
                    };

                    ApiResponse<Order> response = await orderController.OrdersCaptureAsync(capture);
                    ConsoleLog(response, "CaptureOrder");
                    return true;
                }
                catch (ApiException e)
                {
                    Console.WriteLine($"\nPaypalService.CaptureOrder ERROR:\n{e}\n");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"\nPaypalService.CaptureOrder: orderId is null\n");
                return false;
            }
        }

        private void ConsoleLog(ApiResponse<Order> response, string parentMethodName)
        {
            Console.WriteLine($"\nPaypalService.{parentMethodName} RESPONSE\n" +
                    $"HTTP StatusCode: {response.StatusCode}\n" +
                    $"OrderId: {response.Data.Id}\n" +
                    $"Order Status: {response.Data.Status}");

            foreach (LinkDescription link in  response.Data.Links)
            {
                Console.WriteLine($"{link.Rel}: {link.Href}\n");
            }
        }
    }
}
