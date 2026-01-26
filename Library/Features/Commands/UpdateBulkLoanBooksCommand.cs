using EFCore.BulkExtensions;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBulkLoanBooksCommand(IEnumerable<LoanBook> LoanBooks) : IRequest;

    public class UpdateBulkLoanBooksCommandHandler(
        ICommandRepo<LoanBook> loanBookCommandRepo
        ) : IRequestHandler<UpdateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<LoanBook> _loanBookCommandRepo = loanBookCommandRepo;

        public async Task Handle(UpdateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            List<string> includeLoanBookProps = [
                nameof(LoanBook.Id),
                nameof(LoanBook.MemberId),
                nameof(LoanBook.BookId),
                nameof(LoanBook.LoanedDate),
                nameof(LoanBook.ReturnedDate),
                nameof(Root.ModifiedAt)
            ];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeLoanBookProps };
            await _loanBookCommandRepo.BulkUpdateAsync(loanBooks, options1);
        }
    }
}