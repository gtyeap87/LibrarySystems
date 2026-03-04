using Library.Repositories;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateLoanedBookCommand(Models.LoanBook LoanBook) : IRequest<Models.LoanBook>;

    public class UpdateLoanedBookCommandHandler(
        ICommandRepo<Models.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<UpdateLoanedBookCommand, Models.LoanBook>
    {
        public async Task<Models.LoanBook> Handle(UpdateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            return await loanBookCommandRepo.UpdateAsync(loanBook);
        }
    }
}