using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateLoanedBookCommand(Model.LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateLoanedBookCommand, Guid>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = loanBookCommandRepo;

        public async Task<Guid> Handle(CreateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            return await _loanBookCommandRepo.AddAsync(loanBook);
        }
    }
}