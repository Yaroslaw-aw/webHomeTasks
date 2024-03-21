namespace nGraphQL.Mutations
{
    public class MyMutation
    {
        public async Task<AuthorPayload> AddAuthor(AuthorInput input, [Service] Repository repository)
        {
            Author author = new Author(Repository.GetAuthorId(), input.name);
            await repository.AddAuthorAsync(author);
            return new AuthorPayload(author);
        }

        public async Task<BookPayload> AddBook(BookInput input, [Service] Repository repository)
        {
            Author author = await repository.GetAuthorAsync(input.author) ?? throw new Exception("Author not found");
            Book book = new Book(Repository.GetBookId(), input.title, author);
            await repository.AddBookAsync(book);
            return new BookPayload(book);
        }
    }
}
