using StackExchange.Redis;

namespace Market.DTO
{
    public class GetAllProductsDto
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public ulong? Count { get; set; }
        public string? Description { get; set; }
        public string? StorageName { get; set; }
    }
}
