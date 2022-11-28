using PaymentService.Model;

namespace PaymentService
{


    class InventoryClient
    {

        private HttpClient _httpClient;

        public InventoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product> GetProductAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"/product/{productId}");
        }


    }


}