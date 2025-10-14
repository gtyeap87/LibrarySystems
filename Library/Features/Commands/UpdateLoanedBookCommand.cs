using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateLoanedBookCommand(Model.LoanBook LoanedBook) : IRequest<Model.LoanBook>;

    public class UpdateLoanedBookCommandHandler(ICommandRepo<Model.LoanBook> memberCommandRepo) : IRequestHandler<UpdateLoanedBookCommand, Model.LoanBook>
    {
        private readonly ICommandRepo<Model.LoanBook> _memberCommandRepo = memberCommandRepo;

        public async Task<Model.LoanBook> Handle(UpdateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.UpdateAsync(command.LoanedBook);
        }
    }
}