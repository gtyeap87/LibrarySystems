using EFCore.BulkExtensions;
using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBulkLoanBooksCommand(IEnumerable<LoanBook> LoanBooks) : IRequest;

    public class UpdateBulkLoanBooksCommandHandler(
        ICommandRepo<LoanBook> loanBookCommandRepo,
        IValidator<IEnumerable<LoanBook>> validator
        ) : IRequestHandler<UpdateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<LoanBook> _loanBookCommandRepo = loanBookCommandRepo;
        private readonly IValidator<IEnumerable<LoanBook>> _validator = validator;

        public async Task Handle(UpdateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            var result = await _validator.ValidateAsync(loanBooks, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            List<string> includeLoanBookProps = [
                nameof(LoanBook.Id),
                nameof(LoanBook.MemberId),
                nameof(LoanBook.BookId),
                nameof(LoanBook.LoanedDate),
                nameof(LoanBook.ReturnedDate),
                nameof(Root.ModifiedAt)
            ];
            var options = new BulkConfig() { PropertiesToIncludeOnUpdate = includeLoanBookProps };
            await _loanBookCommandRepo.BulkUpdateAsync(loanBooks, options);
        }
    }
}