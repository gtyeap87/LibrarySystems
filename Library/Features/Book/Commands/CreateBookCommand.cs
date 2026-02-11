using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands;

public record CreateBookCommand(BookRequest Request) : IRequest<Guid>;

public class CreateBookCommandHandler(
     ICommandRepo<Model.Book> bookCommandRepo,
     ICommandRepo<BookStock> bookStockCommandRepo
    ) : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly ICommandRepo<Model.Book> _commandRepo = bookCommandRepo;
    private readonly ICommandRepo<BookStock> _bookStockCommandRepo = bookStockCommandRepo;

    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var book = command.Request.Book;
        var qty = command.Request.Qty;
        var newBookId = await _commandRepo.AddAsync(book);

        var bookStock = new BookStock
        {
            BookId = newBookId,
            Quantity = qty
        };
        await _bookStockCommandRepo.AddAsync(bookStock);

        return newBookId;
    }
}