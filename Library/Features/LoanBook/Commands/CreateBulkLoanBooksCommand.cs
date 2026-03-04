using Library.Repositories;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateBulkLoanBooksCommand(IEnumerable<Models.LoanBook> LoanBooks) : IRequest;

    public class CreateBulkLoanBooksCommandHandler(
        ICommandRepo<Models.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateBulkLoanBooksCommand>
    {
        public async Task Handle(CreateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            await loanBookCommandRepo.BulkInsertAsync(loanBooks);
        }
    }
}