using Library.Dto.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record DeleteMemberCommand(Guid MemberId, PaginationRequestDto Page) : IRequest;

    public class DeleteMemberCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo,
        IQueryRepo<Model.Member> memberQueryRepo,
        IQueryRepo<Model.LoanBook> loanBookQueryRepo
            ) : IRequestHandler<DeleteMemberCommand>
    {
        public async Task Handle(DeleteMemberCommand command, CancellationToken cancellationToken)
        {
            var member = await memberQueryRepo.GetByIdAsync(command.MemberId)
                ?? throw new KeyNotFoundException($"Member with ID {command.MemberId} not found.");

            var spec = new FullLoanedBookSpec(null, member.Name);
            var activeLoans = await loanBookQueryRepo.ListAsync(spec, command.Page);

            if (activeLoans.Any())
            {
                throw new InvalidOperationException("Cannot delete member with active loans.");
            }

            await memberCommandRepo.DeleteAsync(member);
        }
    }
}