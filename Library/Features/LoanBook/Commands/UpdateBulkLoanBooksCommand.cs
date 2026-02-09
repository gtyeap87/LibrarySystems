using EFCore.BulkExtensions;
using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateBulkLoanBooksCommand(IEnumerable<Model.LoanBook> LoanBooks) : IRequest;

    public class UpdateBulkLoanBooksCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo,
        IValidator<IEnumerable<Model.LoanBook>> validator
        ) : IRequestHandler<UpdateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = loanBookCommandRepo;
        private readonly IValidator<IEnumerable<Model.LoanBook>> _validator = validator;

        public async Task Handle(UpdateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            var result = await _validator.ValidateAsync(loanBooks, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            List<string> includeLoanBookProps = [
                nameof(Model.LoanBook.Id),
                nameof(Model.LoanBook.MemberId),
                nameof(Model.LoanBook.BookId),
                nameof(Model.LoanBook.LoanedDate),
                nameof(Model.LoanBook.ReturnedDate),
                nameof(Root.ModifiedAt)
            ];
            var options = new BulkConfig() { PropertiesToIncludeOnUpdate = includeLoanBookProps };
            await _loanBookCommandRepo.BulkUpdateAsync(loanBooks, options);
        }
    }
}