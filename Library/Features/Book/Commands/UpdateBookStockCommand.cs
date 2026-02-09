using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record UpdateBookStockCommand(BookStock BookStock) : IRequest<BookStock>;

    public class UpdateBookStockCommandHandler(
        ICommandRepo<BookStock> bookStockCommandRepo,
        IValidator<BookStock> validator
        ) : IRequestHandler<UpdateBookStockCommand, BookStock>
    {
        private readonly ICommandRepo<BookStock> _memberCommandRepo = bookStockCommandRepo;
        private readonly IValidator<BookStock> _validator = validator;

        public async Task<BookStock> Handle(UpdateBookStockCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.BookStock, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await _memberCommandRepo.UpdateAsync(command.BookStock);
        }
    }
}