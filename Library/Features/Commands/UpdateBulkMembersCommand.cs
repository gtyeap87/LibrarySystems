using EFCore.BulkExtensions;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBulkMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class UpdateBulkMembersCommandHandler(
        ICommandRepo<Member> memberCommandRepo
        ) : IRequestHandler<UpdateBulkMembersCommand>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;

        public async Task Handle(UpdateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            var members = command.Members;
            List<string> includeMemberProps = [nameof(Member.Name), nameof(Member.JoinedDate), nameof(Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeMemberProps };
            await _memberCommandRepo.BulkUpdateAsync(members, options1);
        }
    }
}