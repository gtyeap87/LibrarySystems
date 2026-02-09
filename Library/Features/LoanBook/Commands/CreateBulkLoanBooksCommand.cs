using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateBulkLoanBooksCommand(IEnumerable<Model.LoanBook> LoanBooks) : IRequest;

    public class CreateBulkLoanBooksCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo,
        IValidator<IEnumerable<Model.LoanBook>> validator
        ) : IRequestHandler<CreateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = loanBookCommandRepo;
        private readonly IValidator<IEnumerable<Model.LoanBook>> _validator = validator;

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