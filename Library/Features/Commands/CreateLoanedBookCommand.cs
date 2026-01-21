using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateLoanedBookCommand(LoanBook LoanBook) : IRequest<Guid>;

    public class AddLoanedBookCommandHandler(ICommandRepo<LoanBook> memberCommandRepo) : IRequestHandler<CreateLoanedBookCommand, Guid>
    {
        private readonly ICommandRepo<LoanBook> _memberCommandRepo = memberCommandRepo;

        public async Task<Guid> Handle(CreateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.AddAsync(command.LoanBook);
        }
    }
}