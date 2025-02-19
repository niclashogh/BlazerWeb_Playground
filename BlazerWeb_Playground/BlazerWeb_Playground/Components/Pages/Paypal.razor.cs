using API_Playground.Services;
using ModelLibrary;
using BlazerWeb_Playground.Services;

namespace BlazerWeb_Playground.Components.Pages
{
    public partial class Paypal // Manually added by creating new class: name.razor.cs
    {
        protected override void OnInitialized()
        {
            GetCatelog();
        }

        private ProductApiService productApiService = new();
        private PaypalService paypalService = new();
        private List<Product> catelog = new();
        private List<Product> basket = new();

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

        private void Buy()
        {
            paypalService.CreateOrder();
        }
    }
}
