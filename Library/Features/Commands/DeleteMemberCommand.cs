using Library.Model;
using Library.Model.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Commands.Member
{
    public record DeleteMemberCommand(Guid MemberId, PaginationRequest Page) : IRequest;

    public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo;
        private readonly IQueryRepo<Model.Member> _memberQueryRepo;
        private readonly IQueryRepo<LoanBook> _loanBookQueryRepo;

        public DeleteMemberCommandHandler(
            ICommandRepo<Model.Member> memberCommandRepo,
            IQueryRepo<Model.Member> memberQueryRepo,
            IQueryRepo<LoanBook> loanBookQueryRepo
            )
        {
            _memberCommandRepo = memberCommandRepo;
            _memberQueryRepo = memberQueryRepo;
            _loanBookQueryRepo = loanBookQueryRepo;
        }

        public async Task Handle(DeleteMemberCommand command, CancellationToken cancellationToken)
        {
            var member = await _memberQueryRepo.GetByIdAsync(command.MemberId)
                ?? throw new KeyNotFoundException($"Member with ID {command.MemberId} not found.");

            var spec = new FullLoanedBookSpec(null, member.Name);
            var activeLoans = await _loanBookQueryRepo.ListAsync(spec, command.Page);

            if (activeLoans.Any())
            {
                throw new InvalidOperationException("Cannot delete member with active loans.");
            }

            await _memberCommandRepo.DeleteAsync(member);
        }
    }
}