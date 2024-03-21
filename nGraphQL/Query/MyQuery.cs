namespace nGraphQL.Query
{
    public class MyQuery
    {
        public Task<List<Book>> GetBooksAsync([Service] Repository repository) => repository.GetBooksAsync();
        public Task<List<Book>> GetBooksByAurhorId([Service] Repository repository, int aurhorId) => repository.GetBooksAsync(aurhorId);
    }
}
