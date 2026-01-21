using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateBulkBooksCommand(IEnumerable<(Book book, int qty)> Books) : IRequest;

    public class CreateBulkBooksCommandHandler(
        ICommandRepo<Book> bookCommandRepo,
        ICommandRepo<BookStock> bookStockCommandRepo
        ) : IRequestHandler<CreateBulkBooksCommand>
    {
        private readonly ICommandRepo<Book> _bookCommandRepo = bookCommandRepo;
        private readonly ICommandRepo<BookStock> _bookStockCommandRepo = bookStockCommandRepo;

        public async Task Handle(CreateBulkBooksCommand command, CancellationToken cancellationToken)
        {
            var books = command.Books.Select(b => b.book);
            await _bookCommandRepo.BulkInsertAsync(books);

            var bookStocks = command.Books
                .SelectMany(b => Enumerable.Range(0, b.qty).DistinctBy(_ => b.book.Id)
                .Select(_ => new BookStock { BookId = b.book.Id, Quantity = b.qty }));
            await _bookStockCommandRepo.BulkInsertAsync(bookStocks);
        }
    }
}