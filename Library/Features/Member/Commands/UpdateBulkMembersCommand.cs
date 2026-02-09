using EFCore.BulkExtensions;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateBulkMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

    public class UpdateBulkMembersCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo
        ) : IRequestHandler<UpdateBulkMembersCommand>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;

        public async Task Handle(UpdateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            var members = command.Members;
            List<string> includeMemberProps = [nameof(Model.Member.Name), nameof(Model.Member.JoinedDate), nameof(Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeMemberProps };
            await _memberCommandRepo.BulkUpdateAsync(members, options1);
        }
    }
}