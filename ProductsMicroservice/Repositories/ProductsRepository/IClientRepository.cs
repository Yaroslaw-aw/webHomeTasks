using ProductsMicroservice.DTO;

namespace ProductsMicroservice.Repositories.ProductsRepository
{
    public interface IClientRepository
    {
        Task<Guid?> AddClientAsync(ClientDto product);
        Task<IEnumerable<ClientDto>?> GetClientsAsync();
        Task<bool> ClientExistsAsync(Guid clientId);
    }
}
