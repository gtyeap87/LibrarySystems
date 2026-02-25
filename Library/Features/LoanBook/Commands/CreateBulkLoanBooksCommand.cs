using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateBulkLoanBooksCommand(IEnumerable<Model.LoanBook> LoanBooks) : IRequest;

    public class CreateBulkLoanBooksCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateBulkLoanBooksCommand>
    {
        public async Task Handle(CreateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            await loanBookCommandRepo.BulkInsertAsync(loanBooks);
        }
    }
}