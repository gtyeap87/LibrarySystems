using EFCore.BulkExtensions;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBulkBooksCommand(IEnumerable<(Book book, int qty)> Books) : IRequest;

    public class UpdateBulkBooksCommandHandler(
        ICommandRepo<Book> bookCommandRepo,
        ICommandRepo<BookStock> bookStockCommandRepo
        ) : IRequestHandler<UpdateBulkBooksCommand>
    {
        private readonly ICommandRepo<Book> _bookCommandRepo = bookCommandRepo;
        private readonly ICommandRepo<BookStock> _bookStockCommandRepo = bookStockCommandRepo;

        public async Task Handle(UpdateBulkBooksCommand command, CancellationToken cancellationToken)
        {
            var books = command.Books.Select(x => x.book);

            List<string> includeBookProps = [nameof(Book.Genre), nameof(Book.Name), nameof(Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookProps };
            await _bookCommandRepo.BulkUpdateAsync(books, options1);

            var bookStocks = command.Books
                .SelectMany(b => Enumerable.Range(0, b.qty).DistinctBy(_ => b.book.Id)
                .Select(_ => new BookStock { BookId = b.book.Id, Quantity = b.qty }));

            List<string> includeBookStockProps = [nameof(BookStock.Quantity), nameof(Root.ModifiedAt)];
            var options2 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookStockProps };
            await _bookStockCommandRepo.BulkUpdateAsync(bookStocks, options2);
        }
    }
}