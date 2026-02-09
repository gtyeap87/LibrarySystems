using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateLoanedBookCommand(Model.LoanBook LoanBook) : IRequest<Model.LoanBook>;

    public class UpdateLoanedBookCommandHandler(
        ICommandRepo<Model.LoanBook> memberCommandRepo
        ) : IRequestHandler<UpdateLoanedBookCommand, Model.LoanBook>
    {
        private readonly ICommandRepo<Model.LoanBook> _loanBookCommandRepo = memberCommandRepo;

        public async Task<Model.LoanBook> Handle(UpdateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            return await _loanBookCommandRepo.UpdateAsync(loanBook);
        }
    }
}