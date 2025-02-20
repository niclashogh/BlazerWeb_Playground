using API_Playground.Services;
using ModelLibrary;
using BlazerWeb_Playground.Services;
using Microsoft.AspNetCore.Components;

namespace BlazerWeb_Playground.Components.Pages
{
    public partial class Paypal // Manually added by creating new class: name.razor.cs
    {
        private NavigationManager navigationManager;
        private ProductApiService productApiService;
        private PaypalService paypalService;
        private List<Product> catelog = new();
        private List<Product> basket = new();

        public Paypal(PaypalService paypalService, NavigationManager navManager)
        {
            this.paypalService = paypalService;
            navigationManager = navManager;
        }

        protected override void OnInitialized()
        {
            GetCatelog();
        }

        private async void GetCatelog()
        {
            catelog = await productApiService.GetCatelog();
            this.StateHasChanged();
        }

        private void AddToBasket(int id)
        {
            Product? result = catelog.FirstOrDefault(sorting => sorting.Id == id);

            if (result != null)
            {
                basket.Add(result);
                this.StateHasChanged();
            }
        }

        private async Task Buy()
        {
            double totalPrice = 0;

            foreach (Product product in basket)
            {
                totalPrice += product.Price;
            }

            await paypalService.CreateOrder(totalPrice);

            if (!string.IsNullOrEmpty(paypalService.OrderApproveUrl))
            {
                navigationManager.NavigateTo(paypalService.OrderApproveUrl, forceLoad: true);
            }
        }
    }
}
