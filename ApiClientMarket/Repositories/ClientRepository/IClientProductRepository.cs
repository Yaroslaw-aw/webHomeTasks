using ApiClientMarket.Dto;

namespace ApiClientMarket.Repositories.ClientRepository
{
    public interface IClientProductRepository
    {
        public Task<Guid?> BuyProductAsync(ClientProductDto productDto);
        public Task<Guid?> ReturnProductAsync(Guid returnProductId);
        public Task<IEnumerable<Guid?>?> ListProductsAsync(Guid clientId);
    }
}
