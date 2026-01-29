using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateBulkLoanBooksCommand(IEnumerable<LoanBook> LoanBooks) : IRequest;

    public class CreateBulkLoanBooksCommandHandler(
        ICommandRepo<LoanBook> loanBookCommandRepo,
        IValidator<IEnumerable<LoanBook>> validator
        ) : IRequestHandler<CreateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<LoanBook> _loanBookCommandRepo = loanBookCommandRepo;
        private readonly IValidator<IEnumerable<LoanBook>> _validator = validator;

        public async Task Handle(CreateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            var result = await _validator.ValidateAsync(loanBooks, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _loanBookCommandRepo.BulkInsertAsync(loanBooks);
        }
    }
}