using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateBulkMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class CreateBulkMembersCommandHandler(ICommandRepo<Member> memberCommandRepo) : IRequestHandler<CreateBulkMembersCommand>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;

        public async Task Handle(CreateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            await _memberCommandRepo.BulkInsertAsync(command.Members);
        }
    }
}