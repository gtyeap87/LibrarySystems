using EFCore.BulkExtensions;
using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateBulkMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

    public class UpdateBulkMembersCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo,
        IValidator<IEnumerable<Model.Member>> validator
        ) : IRequestHandler<UpdateBulkMembersCommand>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<IEnumerable<Model.Member>> _validator = validator;

        public async Task Handle(UpdateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Members, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var members = command.Members;
            List<string> includeMemberProps = [nameof(Model.Member.Name), nameof(Model.Member.JoinedDate), nameof(Root.ModifiedAt)];
            var options1 = new BulkConfig() { PropertiesToIncludeOnUpdate = includeMemberProps };
            await _memberCommandRepo.BulkUpdateAsync(members, options1);
        }
    }
}