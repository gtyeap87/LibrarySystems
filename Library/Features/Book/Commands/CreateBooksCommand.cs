using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands;

public record CreateBooksCommand(BooksRequest Request) : IRequest;

public class CreateBooksCommandHandler(
    ICommandRepo<Model.Book> bookCommandRepo,
    ICommandRepo<BookStock> bookStockCommandRepo
    ) : IRequestHandler<CreateBooksCommand>
{
    public async Task Handle(CreateBooksCommand command, CancellationToken cancellationToken)
    {
        var books = command.Request.Books.Select(b => b.Book);
        await bookCommandRepo.AddRangeAsync(books);

        var bookStocks = command.Request.Books
           .SelectMany(b => Enumerable.Range(0, b.Qty).DistinctBy(_ => b.Book.Id)
           .Select(_ => new BookStock { BookId = b.Book.Id, Quantity = b.Qty }));
        await bookStockCommandRepo.AddRangeAsync(bookStocks);
    }
}