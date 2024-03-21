using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProductsMicroservice.DTO;
using ProductsMicroservice.Repositories.ProductsRepository;

namespace ProductsMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientRepository repository;
        private readonly IMemoryCache cache;

        public ClientController(IClientRepository repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        [HttpPost(template: "AddClient")]
        public async Task<ActionResult<Guid?>> AddClient(ClientDto productsDto)
        {
            Guid? newProductId = await repository.AddClientAsync(productsDto);
            cache.Remove("products");
            return CreatedAtAction(nameof(AddClient), newProductId);
        }

        [HttpGet(template: "GetClients")]
        public async Task<ActionResult<IEnumerable<ClientDto>?>> GetClients()
        {
            if (cache.TryGetValue("products", out List<ClientDto>? productsCache) && productsCache != null)
                return AcceptedAtAction(nameof(GetClients), productsCache);

            IEnumerable<ClientDto>? allProductsDtos =  await repository.GetClientsAsync();

            if (allProductsDtos != null)
                cache.Set("products", allProductsDtos, TimeSpan.FromMinutes(30));

            return AcceptedAtAction(nameof(GetClients), allProductsDtos);
        }

        [HttpGet(template: "ClientExists")]
        public async Task<ActionResult<bool>> ClientExists(Guid clientId)
        {
            return AcceptedAtAction(nameof(ClientExists), await repository.ClientExistsAsync(clientId));
        }
    }
}
