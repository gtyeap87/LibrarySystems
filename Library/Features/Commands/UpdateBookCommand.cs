using FluentValidation;
using Library.Model;
using Library.Model.Request;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBookCommand(BookRequest Request) : IRequest<Model.Book>;

    public class UpdateBookCommandHandler(
        ICommandRepo<Book> bookCommandRepo,
        IValidator<BookRequest> validator
        ) : IRequestHandler<UpdateBookCommand, Model.Book>
    {
        private readonly ICommandRepo<Model.Book> _memberCommandRepo = bookCommandRepo;
        private readonly IValidator<BookRequest> _validator = validator;

        public async Task<Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Request, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await _memberCommandRepo.UpdateAsync(command.Request.Book);
        }
    }
}