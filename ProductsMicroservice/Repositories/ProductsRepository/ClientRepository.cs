using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsMicroservice.Db;
using ProductsMicroservice.DTO;

namespace ProductsMicroservice.Repositories.ProductsRepository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public ClientRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Guid?> AddClientAsync(ClientDto product)
        {
            Client newProduct = mapper.Map<Client>(product);

            using (context)
            {
                context.Clients.Add(newProduct);
                await context.SaveChangesAsync();
            }

            return newProduct.Id;
        }

        public async Task<bool> ClientExistsAsync(Guid clientId)
        {
            using (context)            
                return await context.Clients.AnyAsync(client => client.Id == clientId);
            
        }

        public async Task<IEnumerable<ClientDto>?> GetClientsAsync()
        {
            List<Client> allProducts = new List<Client>();

            using (context)            
                allProducts = await context.Clients.AsNoTracking().ToListAsync();            

            List<ClientDto> productsDto = new List<ClientDto>();
            productsDto = mapper.Map(allProducts, productsDto);

            return productsDto;
        }
    }
}
