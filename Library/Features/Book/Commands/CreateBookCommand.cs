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
    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var book = command.Request.Book;
        var qty = command.Request.Qty;
        var newBookId = await bookCommandRepo.AddAsync(book);

        var bookStock = new BookStock
        {
            BookId = newBookId,
            Quantity = qty
        };
        await bookStockCommandRepo.AddAsync(bookStock);

        return newBookId;
    }
}