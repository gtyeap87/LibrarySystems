using FluentValidation;
using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands;

public record CreateBooksCommand(BooksRequest Request) : IRequest;

public class CreateBooksCommandHandler(
    ICommandRepo<Book> bookCommandRepo,
    ICommandRepo<BookStock> bookStockCommandRepo,
    IValidator<BooksRequest> validator
    ) : IRequestHandler<CreateBooksCommand>
{
    private readonly ICommandRepo<Book> _bookCommandRepo = bookCommandRepo;
    private readonly ICommandRepo<BookStock> _bookStockCommandRepo = bookStockCommandRepo;
    private readonly IValidator<BooksRequest> _validator = validator;

    public async Task Handle(CreateBooksCommand command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        var books = command.Request.Books.Select(b => b.Book);
        await _bookCommandRepo.AddRangeAsync(books);

        var bookStocks = command.Request.Books
           .SelectMany(b => Enumerable.Range(0, b.Qty).DistinctBy(_ => b.Book.Id)
               .Select(_ => new BookStock { BookId = b.Book.Id, Quantity = b.Qty }));
        await _bookStockCommandRepo.AddRangeAsync(bookStocks);
    }
}