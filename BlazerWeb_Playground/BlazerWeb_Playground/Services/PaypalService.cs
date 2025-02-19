using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Exceptions;

namespace BlazerWeb_Playground.Services
{
    public class PaypalService // Toturial at: https://developer.paypal.com/serversdk/http/getting-started/how-to-get-started/
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

        public async Task CreateOrder(double totalPrice)
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

                Console.WriteLine($"\nPaypalService.CreateOrder RESPONSE (OrderId): {response.Data.Id}\n");
            }
            catch (ApiException e)
            {
                Console.WriteLine($"\nPaypalService.CreateOrder ERROR: {e}\n");
            }
        }

        public async Task<bool> IsOrderCaptured()
        {
            if (!string.IsNullOrEmpty(Order.Id))
            {
                try
                {
                    OrdersCaptureInput capture = new OrdersCaptureInput { Id = Order.Id };
                    ApiResponse<Order> response = await orderController.OrdersCaptureAsync(capture);

                    if (response.Data.Status == OrderStatus.Completed)
                    {
                        Console.WriteLine($"\nPaypalService.IsOrderCaptued RESPONSE (OrderStatus): {response.Data.Status}\n");
                        return true;
                    }
                    else return false;
                }
                catch (ApiException e)
                {
                    Console.WriteLine($"PaypalService.IsOrderCaptured ERROR: {e}\n");
                    return false;
                }
            }
            else return false;
        }
    }
}
