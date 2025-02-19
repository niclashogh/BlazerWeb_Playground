using ModelLibrary;
using Newtonsoft.Json;

namespace API_Playground.Services
{
    public class ProductApiService
    {
        private HttpClient httpClient = new();

        public ProductApiService()
        {
            httpClient.BaseAddress = new Uri("https://localhost:7295");
        }

        public async Task<List<Product>> GetCatelog()
        {
            string apiRoute = "/api/catelog";
            HttpResponseMessage httpResponse = await httpClient.GetAsync(apiRoute);

            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonSerialized = await httpResponse.Content.ReadAsStringAsync();
                List<Product>? products = JsonConvert.DeserializeObject<List<Product>>(jsonSerialized);

                if (products != null && products.Count > 0)
                {
                    return products;
                }
                else return new();
            }
            else return new(); // Could log statuscode
        }

        public async Task<Product> GetProduct(int id)
        {
            string apiRoute = $"/api/catelog/{id}";
            HttpResponseMessage httpResponse = await httpClient.GetAsync(apiRoute);

            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonSerialized = await httpResponse.Content.ReadAsStringAsync();
                Product? product = JsonConvert.DeserializeObject<Product>(jsonSerialized);

                if (product != null)
                {
                    return product;
                }
                else return new();
            }
            else return new(); // Could log statuscode
        }
    }
}
