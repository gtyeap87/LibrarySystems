using EFCore.BulkExtensions;
using FluentValidation;
using Library.Model;
using Library.Model.Request;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBulkBooksCommand(BooksRequest Request) : IRequest;

    public class UpdateBulkBooksCommandHandler(
        ICommandRepo<Book> bookCommandRepo,
        ICommandRepo<BookStock> bookStockCommandRepo,
        IValidator<BooksRequest> validator
        ) : IRequestHandler<UpdateBulkBooksCommand>
    {
        private readonly ICommandRepo<Book> _bookCommandRepo = bookCommandRepo;
        private readonly ICommandRepo<BookStock> _bookStockCommandRepo = bookStockCommandRepo;
        private readonly IValidator<BooksRequest> _validator = validator;

        public async Task Handle(UpdateBulkBooksCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Request, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var books = command.Request.Books.Select(x => x.Book);

            List<string> includeBookProps = [nameof(Book.Genre), nameof(Book.Name), nameof(Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookProps };
            await _bookCommandRepo.BulkUpdateAsync(books, options1);

            var bookStocks = command.Request.Books
                .SelectMany(b => Enumerable.Range(0, b.Qty).DistinctBy(_ => b.Book.Id)
                .Select(_ => new BookStock { BookId = b.Book.Id, Quantity = b.Qty }));

            List<string> includeBookStockProps = [nameof(BookStock.Quantity), nameof(Root.ModifiedAt)];
            var options2 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeBookStockProps };
            await _bookStockCommandRepo.BulkUpdateAsync(bookStocks, options2);
        }
    }
}