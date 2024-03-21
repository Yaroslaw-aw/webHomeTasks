using ApiClientMarket.Db;
using ApiClientMarket.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiClientMarket.Repositories.ClientRepository
{
    public class ClientProductRepository : IClientProductRepository
    {
        private readonly ClientMarketContext context;
        private readonly IMapper mapper;

        public ClientProductRepository(ClientMarketContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Guid?> BuyProductAsync(ClientProductDto productDto)
        {
            ClientProduct? clientProduct = mapper.Map<ClientProduct>(productDto);

            using (context)
            {
                context.Add(clientProduct);
                await context.SaveChangesAsync();
            }
            return clientProduct.ProductId;
        }

        public async Task<IEnumerable<Guid?>?> ListProductsAsync(Guid clientId)
        {
            using (context)
                return await context.ClientProducts.Where(x => x.ClientId == clientId).Select(x => x.ProductId).ToListAsync();
        }

        public async Task<Guid?> ReturnProductAsync(Guid returnProductId)
        {
            using (context)
            {
                ClientProduct returnedProduct = context.ClientProducts.First(x => x.ProductId == returnProductId);

                context.ClientProducts.Remove(returnedProduct);

                await context.SaveChangesAsync();

                return returnedProduct.ProductId;
            }
        }
    }
}
