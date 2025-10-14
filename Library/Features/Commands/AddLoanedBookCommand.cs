using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record AddLoanedBookCommand(LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(ICommandRepo<LoanBook> memberCommandRepo) : IRequestHandler<AddLoanedBookCommand, Guid>
    {
        private readonly ICommandRepo<LoanBook> _memberCommandRepo = memberCommandRepo;

        public async Task<Guid> Handle(AddLoanedBookCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.AddAsync(command.LoanBook);
        }
    }
}