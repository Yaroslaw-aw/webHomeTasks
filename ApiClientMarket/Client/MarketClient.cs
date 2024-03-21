
namespace ApiClientMarket.Client
{
    public class MarketClient : IMarketClient
    {
        private readonly HttpClient client = new HttpClient();
        public async Task<bool> ClientExistsAsync(Guid clientId)
        {
            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7145/Client/ClientExists?clientId={clientId.ToString()}");

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
