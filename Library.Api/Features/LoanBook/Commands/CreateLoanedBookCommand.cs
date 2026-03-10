using Library.Api.Repositories;
using MediatR;

namespace Library.Api.Features.LoanBook.Commands
{
    public record CreateLoanedBookCommand(Models.LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(
        ICommandRepo<Models.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<CreateLoanedBookCommand, Guid>
    {
        public async Task<Guid> Handle(CreateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            return await loanBookCommandRepo.AddAsync(loanBook);
        }
    }
}