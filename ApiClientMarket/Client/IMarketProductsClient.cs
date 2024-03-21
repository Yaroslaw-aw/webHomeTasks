namespace ApiClientMarket.Client
{
    public interface IMarketProductsClient
    {
        public Task<bool> ProductExistsAsync(Guid clientId);
    }
}
