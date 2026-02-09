using EFCore.BulkExtensions;
using Library.Dto.Request;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record UpdateBulkBooksCommand(BooksRequest Request) : IRequest;

    public class UpdateBulkBooksCommandHandler(
        ICommandRepo<Model.Book> bookCommandRepo,
        ICommandRepo<Model.BookStock> bookStockCommandRepo
        ) : IRequestHandler<UpdateBulkBooksCommand>
    {
        private readonly ICommandRepo<Model.Book> _bookCommandRepo = bookCommandRepo;
        private readonly ICommandRepo<Model.BookStock> _bookStockCommandRepo = bookStockCommandRepo;

        public async Task Handle(UpdateBulkBooksCommand command, CancellationToken cancellationToken)
        {
            var books = command.Request.Books.Select(x => x.Book);

            List<string> includeBookProps = [nameof(Model.Book.Genre), nameof(Model.Book.Name), nameof(Model.Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookProps };
            await _bookCommandRepo.BulkUpdateAsync(books, options1);

            var bookStocks = command.Request.Books
                .SelectMany(b => Enumerable.Range(0, b.Qty).DistinctBy(_ => b.Book.Id)
                .Select(_ => new Model.BookStock { BookId = b.Book.Id, Quantity = b.Qty }));

            List<string> includeBookStockProps = [nameof(Model.BookStock.Quantity), nameof(Model.Root.ModifiedAt)];
            var options2 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookStockProps };
            await _bookStockCommandRepo.BulkUpdateAsync(bookStocks, options2);
        }
    }
}