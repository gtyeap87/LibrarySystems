using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands;

public record CreateBooksCommand(IEnumerable<(Book book, int qty)> Books) : IRequest;

public class CreateBooksCommandHandler(
    ICommandRepo<Book> bookCommandRepo,
    ICommandRepo<BookStock> bookStockCommandRepo
    ) : IRequestHandler<CreateBooksCommand>
{
    private readonly ICommandRepo<Book> _bookCommandRepo = bookCommandRepo;
    private readonly ICommandRepo<BookStock> _bookStockCommandRepo = bookStockCommandRepo;

    public async Task Handle(CreateBooksCommand command, CancellationToken cancellationToken)
    {
        var books = command.Books.Select(b => b.book);
        await _bookCommandRepo.AddRangeAsync(books);

        var bookStocks = command.Books
           .SelectMany(b => Enumerable.Range(0, b.qty).DistinctBy(_ => b.book.Id)
               .Select(_ => new BookStock { BookId = b.book.Id, Quantity = b.qty }));
        await _bookStockCommandRepo.AddRangeAsync(bookStocks);
    }
}