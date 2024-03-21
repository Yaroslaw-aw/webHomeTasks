using nGraphQL.Mutations;
using nGraphQL.Query;

namespace nGraphQL
{

    public record Author(int Id, string Name);
    public record Book(int Id, string Title, Author Author);

    public record BookPayload(Book? record, string? error = null);
    public record BookInput(string title, int author);
    public record AuthorPayload(Author record);
    public record AuthorInput(string name);







    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddSingleton<Repository>()
                .AddGraphQLServer()
                .AddQueryType<MyQuery>()
                .AddMutationType<MyMutation>();

            var app = builder.Build();

            app.MapGraphQL();

            app.Run();
        }
    }

    public class Repository
    {
        public static int GetAuthorId() => authors.Last().Id + 1;
        public static int GetBookId() => books.Last().Id + 1;

        private static List<Author> authors = new List<Author>()
        {
            new Author(0, "Автор 0"),
            new Author(1, "Автор 1"),
            new Author(2, "Автор 2")
        };

        private static List<Book> books = new List<Book>()
        {
            new Book(0, "Книга1", authors[0]),
            new Book(1, "Книга2", authors[1]),
            new Book(2, "Книга3", authors[2]),
            new Book(3, "Книга4", authors[2]),
        };

        public Task<List<Book>> GetBooksAsync()
        {
            return Task.FromResult(books);
        }

        public Task<List<Book>> GetBooksAsync(int authorId)
        {
            return Task.FromResult(books.Where(x => x.Author.Id == authorId).ToList());
        }

        public Task<Author?> GetAuthorAsync(int authorId)
        {
            return Task.FromResult(authors.FirstOrDefault(author => author.Id == authorId));
        }

        public Task AddAuthorAsync(Author author)
        {
            authors.Add(author);
            return Task.CompletedTask;
        }

        public Task AddBookAsync(Book book)
        {
            books.Add(book);
            return Task.CompletedTask;
        }


    }
}
