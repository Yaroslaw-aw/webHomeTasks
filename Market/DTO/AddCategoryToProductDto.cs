namespace Market.DTO
{
    public class AddCategoryToProductDto
    {
        public Guid? productId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
