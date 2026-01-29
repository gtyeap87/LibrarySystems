using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateLoanedBookCommand(LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(
        ICommandRepo<LoanBook> memberCommandRepo,
        IValidator<LoanBook> validator
        ) : IRequestHandler<CreateLoanedBookCommand, Guid>
    {
        private readonly ICommandRepo<LoanBook> _loanBookCommandRepo = memberCommandRepo;
        private readonly IValidator<LoanBook> _validator = validator;

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