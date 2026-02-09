using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateLoanedBookCommand(Model.LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(
        ICommandRepo<Model.LoanBook> memberCommandRepo,
        IValidator<Model.LoanBook> validator
        ) : IRequestHandler<CreateLoanedBookCommand, Guid>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = memberCommandRepo;
        private readonly IValidator<Model.LoanBook> _validator = validator;

        public async Task<Guid> Handle(CreateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;
            var result = await _validator.ValidateAsync(loanBook, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await _loanBookCommandRepo.AddAsync(loanBook);
        }
    }
}