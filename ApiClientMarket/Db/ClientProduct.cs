using System.ComponentModel.DataAnnotations;

namespace ApiClientMarket.Db
{
    public class ClientProduct
    {
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        
        public Guid? ProductId { get; set; }
    }
}
