using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateBulkLoanBooksCommand(IEnumerable<Model.LoanBook> LoanBooks) : IRequest;

    public class CreateBulkLoanBooksCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateBulkLoanBooksCommand>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = loanBookCommandRepo;

        public async Task Handle(CreateBulkLoanBooksCommand command, CancellationToken cancellationToken)
        {
            var loanBooks = command.LoanBooks;

            await _loanBookCommandRepo.BulkInsertAsync(loanBooks);
        }
    }
}