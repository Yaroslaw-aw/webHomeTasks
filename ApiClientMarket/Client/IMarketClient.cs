namespace ApiClientMarket.Client
{
    public interface IMarketClient
    {
        public Task<bool> ClientExistsAsync(Guid clientId);
    }
}
