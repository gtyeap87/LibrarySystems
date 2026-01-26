using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateBulkLoanBooksCommand(IEnumerable<LoanBook> LoanBooks) : IRequest;

    public class CreateBulkLoanBooksCommandHandler(
        ICommandRepo<LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<LoanBook> _loanBookCommandRepo = loanBookCommandRepo;

        public async Task Handle(CreateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;
            await _loanBookCommandRepo.BulkInsertAsync(loanBooks);
        }
    }
}