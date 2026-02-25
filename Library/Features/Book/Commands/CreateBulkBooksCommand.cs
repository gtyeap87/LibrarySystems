using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record CreateBulkBooksCommand(BooksRequest Request) : IRequest;

    public class CreateBulkBooksCommandHandler(
        ICommandRepo<Model.Book> bookCommandRepo,
        ICommandRepo<BookStock> bookStockCommandRepo
        ) : IRequestHandler<CreateBulkBooksCommand>
    {
        public async Task Handle(CreateBulkBooksCommand command, CancellationToken cancellationToken)
        {
            var books = command.Request.Books.Select(b => b.Book);
            await bookCommandRepo.BulkInsertAsync(books);

            var bookStocks = command.Request.Books
                .SelectMany(b => Enumerable.Range(0, b.Qty).DistinctBy(_ => b.Book.Id)
                .Select(_ => new BookStock { BookId = b.Book.Id, Quantity = b.Qty }));
            await bookStockCommandRepo.BulkInsertAsync(bookStocks);
        }
    }
}