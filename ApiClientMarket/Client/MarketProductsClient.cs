
namespace ApiClientMarket.Client
{
    public class MarketProductsClient : IMarketProductsClient
    {
        private readonly HttpClient client = new HttpClient();
        public async Task<bool> ProductExistsAsync(Guid productId)
        {
            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7177/Product/ExistsProduct?productId={productId.ToString()}");

            response.EnsureSuccessStatusCode(); // проверяет ответ на правильность

            string responseBody = await response.Content.ReadAsStringAsync();

            if (responseBody == "true")
            {
                return true;
            }

            if (responseBody == "false")
            {
                return false;
            }

            throw new Exception("Unknown response");
        }
    }
}
