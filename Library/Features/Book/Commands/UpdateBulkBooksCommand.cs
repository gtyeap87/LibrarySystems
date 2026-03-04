using EFCore.BulkExtensions;
using Library.Dto.Requests;
using Library.Repositories;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record UpdateBulkBooksCommand(BooksRequest Request) : IRequest;

    public class UpdateBulkBooksCommandHandler(
        ICommandRepo<Models.Book> bookCommandRepo,
        ICommandRepo<Models.BookStock> bookStockCommandRepo
        ) : IRequestHandler<UpdateBulkBooksCommand>
    {
        public async Task Handle(UpdateBulkBooksCommand command, CancellationToken cancellationToken)
        {
            var books = command.Request.Books.Select(x => x.Book);

            List<string> includeBookProps = [nameof(Models.Book.Genre), nameof(Models.Book.Name), nameof(Models.Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookProps };
            await bookCommandRepo.BulkUpdateAsync(books, options1);

            var bookStocks = command.Request.Books
                .SelectMany(b => Enumerable.Range(0, b.Qty).DistinctBy(_ => b.Book.Id)
                .Select(_ => new Models.BookStock { BookId = b.Book.Id, Quantity = b.Qty }));

            List<string> includeBookStockProps = [nameof(Models.BookStock.Quantity), nameof(Models.Root.ModifiedAt)];
            var options2 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookStockProps };
            await bookStockCommandRepo.BulkUpdateAsync(bookStocks, options2);
        }
    }
}