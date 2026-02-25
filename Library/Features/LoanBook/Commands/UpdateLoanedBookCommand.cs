using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateLoanedBookCommand(Model.LoanBook LoanBook) : IRequest<Model.LoanBook>;

    public class UpdateLoanedBookCommandHandler(
        ICommandRepo<Model.LoanBook> loanBookCommandRepo
        ) : IRequestHandler<UpdateLoanedBookCommand, Model.LoanBook>
    {
        public async Task<Model.LoanBook> Handle(UpdateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            return await loanBookCommandRepo.UpdateAsync(loanBook);
        }
    }
}