using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands;

public record AddBookCommand(Book Book, int Qty) : IRequest<Guid>;

public class AddBookCommandHandler(ILibraryCommandRepository commandRepo) : IRequestHandler<AddBookCommand, Guid>
{
    private readonly ILibraryCommandRepository _commandRepo = commandRepo;

    public async Task<Guid> Handle(AddBookCommand command, CancellationToken cancellationToken)
    {
        var book = command.Book;
        var qty = command.Qty;
        return await _commandRepo.AddBookAsync(book, qty);
    }
}