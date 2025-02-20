using BlazerWeb_Playground.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazerWeb_Playground.Components.Pages
{
    public partial class PaypalReceipt
    {
        private PaypalService paypalService;
        private NavigationManager navigationManager;

        public PaypalReceipt(PaypalService paypalService, NavigationManager navManager)
        {
            this.paypalService = paypalService;
            this.navigationManager = navManager;
        }

        protected override async void OnInitialized()
        {
            Uri uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("token", out var orderId))
            {
                await paypalService.CaptureOrder(orderId);
            }
        }
    }
}
