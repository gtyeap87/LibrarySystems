using Library.Dto.Request;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands;

public record CreateBookCommand(BookRequest Request) : IRequest<Guid>;

public class CreateBookCommandHandler(
    ILibraryCommandRepository commandRepo
    ) : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly ILibraryCommandRepository _commandRepo = commandRepo;

    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var book = command.Request.Book;
        var qty = command.Request.Qty;
        return await _commandRepo.AddBookAsync(book, qty);
    }
}