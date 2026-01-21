using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateBulkMemberCommand(IEnumerable<Member> Members) : IRequest;

    public class AddBulkMemberCommandHandler(ICommandRepo<Member> memberCommandRepo) : IRequestHandler<CreateBulkMemberCommand>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;

        public async Task Handle(CreateBulkMemberCommand command, CancellationToken cancellationToken)
        {
            await _memberCommandRepo.BulkInsertAsync(command.Members);
        }
    }
}