using EFCore.BulkExtensions;
using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBulkMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class UpdateBulkMembersCommandHandler(
        ICommandRepo<Member> memberCommandRepo,
        IValidator<IEnumerable<Member>> validator
        ) : IRequestHandler<UpdateBulkMembersCommand>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<IEnumerable<Member>> _validator = validator;

        public async Task Handle(UpdateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Members, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var members = command.Members;
            List<string> includeMemberProps = [nameof(Member.Name), nameof(Member.JoinedDate), nameof(Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeMemberProps };
            await _memberCommandRepo.BulkUpdateAsync(members, options1);
        }
    }
}