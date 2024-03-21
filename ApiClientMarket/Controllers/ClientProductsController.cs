using ApiClientMarket.Client;
using ApiClientMarket.Dto;
using ApiClientMarket.Repositories.ClientRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;

namespace ApiClientMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientProductsController : Controller
    {
        private readonly IClientProductRepository repository;
        private readonly IMemoryCache cache;

        public ClientProductsController(IClientProductRepository repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        [HttpPost(template: "BuyProduct")]
        public async Task<ActionResult<BuyProductResultDto>> BuyProductAsync(ClientProductDto order)
        {
            Task<bool> clientExistTask = new MarketClient().ClientExistsAsync(order.ClientId);
            Task<bool> productExistTask = new MarketProductsClient().ProductExistsAsync(order.ProductId);

            await Task.WhenAll(clientExistTask, productExistTask);

            bool clientExists = await clientExistTask;
            bool productExists = await productExistTask;

            if (clientExists && productExists)
            {
                try
                {
                    await repository.BuyProductAsync(order);
                    return new BuyProductResultDto { Success = true };
                }
                catch (Exception ex)
                {
                    if (ex is DbUpdateException && ex.InnerException is PostgresException && ex?.InnerException?.Message?.Contains("duplicate") == true)
                    {
                        return new BuyProductResultDto { Error = ex.InnerException?.Message };
                    }
                    throw;
                }
            }
            else
            {
                if (!clientExists)
                    return new BuyProductResultDto { Error = "Клиент не найден!" };
                else
                    return new BuyProductResultDto { Error = "Продукт не найден!" };
            }
        }



        //[HttpPost(template: "BuyProduct")]
        //public async Task<ActionResult<Guid?>> BuyProduct(ClientProductDto productDto)
        //{
        //    Guid? boughtProduct = await repository.BuyProductAsync(productDto);
        //    cache.Remove("products");
        //    return CreatedAtAction(nameof(BuyProduct), boughtProduct);
        //}


        [HttpDelete(template: "ReturnProduct")]
        public async Task<ActionResult<Guid?>> ReturnProduct(Guid returnProductId)
        {
            Guid? returnedProduct = await repository.ReturnProductAsync(returnProductId);
            cache.Remove("products");
            return AcceptedAtAction(nameof(ReturnProduct), returnedProduct);
        }


        [HttpGet(template: "ListProducts")]
        public async Task<ActionResult<IEnumerable<Guid?>?>> ListProducts(Guid clientId)
        {
            if (cache.TryGetValue("products", out List<Guid>? productsGuidsCache) && productsGuidsCache != null)
                return AcceptedAtAction(nameof(ListProducts), clientId, productsGuidsCache);

            IEnumerable<Guid?>? productsGuids = await repository.ListProductsAsync(clientId);

            cache.Set("products", productsGuids, TimeSpan.FromMinutes(30));

            return AcceptedAtAction(nameof(ListProducts), productsGuids);
        }
    }
}
