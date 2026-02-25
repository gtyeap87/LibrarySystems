using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record CreateLoanedBookCommand(Model.LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateLoanedBookCommand, Guid>
    {
        public async Task<Guid> Handle(CreateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            return await loanBookCommandRepo.AddAsync(loanBook);
        }
    }
}