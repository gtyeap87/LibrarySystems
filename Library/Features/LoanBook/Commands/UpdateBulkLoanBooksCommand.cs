using EFCore.BulkExtensions;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateBulkLoanBooksCommand(IEnumerable<Model.LoanBook> LoanBooks) : IRequest;

    public class UpdateBulkLoanBooksCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<UpdateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = loanBookCommandRepo;

        public async Task Handle(UpdateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

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