using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record AddBulkMemberCommand(IEnumerable<Member> Members) : IRequest;

    public class AddBulkMemberCommandHandler(ICommandRepo<Member> memberCommandRepo) : IRequestHandler<AddBulkMemberCommand>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;

        public async Task Handle(AddBulkMemberCommand command, CancellationToken cancellationToken)
        {
            await _memberCommandRepo.BulkInsertAsync(command.Members);
        }
    }
}