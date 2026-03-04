using EFCore.BulkExtensions;
using Library.Models;
using Library.Repositories;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateBulkLoanBooksCommand(IEnumerable<Models.LoanBook> LoanBooks) : IRequest;

    public class UpdateBulkLoanBooksCommandHandler(
        ICommandRepo<Models.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<UpdateBulkLoanBooksCommand>
    {
        public async Task Handle(UpdateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            List<string> includeLoanBookProps = [
                nameof(Models.LoanBook.Id),
                nameof(Models.LoanBook.MemberId),
                nameof(Models.LoanBook.BookId),
                nameof(Models.LoanBook.LoanedDate),
                nameof(Models.LoanBook.ReturnedDate),
                nameof(Root.ModifiedAt)
            ];
            var options = new BulkConfig() { PropertiesToIncludeOnUpdate = includeLoanBookProps };
            await loanBookCommandRepo.BulkUpdateAsync(loanBooks, options);
        }
    }
}