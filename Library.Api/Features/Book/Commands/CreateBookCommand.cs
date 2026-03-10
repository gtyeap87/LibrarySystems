using Library.Api.Dto.Requests;
using Library.Api.Models;
using Library.Api.Repositories;
using MediatR;

namespace Library.Api.Features.Book.Commands;

public record CreateBookCommand(BookRequest Request) : IRequest<Guid>;

public class CreateBookCommandHandler(
     ICommandRepo<Models.Book> bookCommandRepo,
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