using EFCore.BulkExtensions;
using Library.Models;
using Library.Repositories;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateBulkMembersCommand(IEnumerable<Models.Member> Members) : IRequest;

    public class UpdateBulkMembersCommandHandler(
        ICommandRepo<Models.Member> memberCommandRepo
        ) : IRequestHandler<UpdateBulkMembersCommand>
    {
        public async Task Handle(UpdateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            var members = command.Members;
            List<string> includeMemberProps = [nameof(Models.Member.Name), nameof(Models.Member.JoinedDate), nameof(Root.ModifiedAt)];
            var opt = new BulkConfig() { PropertiesToIncludeOnUpdate = includeMemberProps };
            await memberCommandRepo.BulkUpdateAsync(members, opt);
        }
    }
}