using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateBulkMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

    public class CreateBulkMembersCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo,
        IValidator<IEnumerable<Model.Member>> validator
        ) : IRequestHandler<CreateBulkMembersCommand>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<IEnumerable<Model.Member>> _validator = validator;

        public async Task Handle(CreateBulkMembersCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Members, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _memberCommandRepo.BulkInsertAsync(command.Members);
        }
    }
}