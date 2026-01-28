using FluentValidation;
using Library.Model.Request;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands;

public record CreateBookCommand(BookRequest Request) : IRequest<Guid>;

public class AddBookCommandHandler(
    ILibraryCommandRepository commandRepo,
    IValidator<BookRequest> validator
    ) : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly ILibraryCommandRepository _commandRepo = commandRepo;
    private readonly IValidator<BookRequest> _validator = validator;

    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        var book = command.Request.Book;
        var qty = command.Request.Qty;
        return await _commandRepo.AddBookAsync(book, qty);
    }
}