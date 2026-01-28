using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateBulkMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class CreateBulkMembersCommandHandler(
        ICommandRepo<Member> memberCommandRepo,
        IValidator<IEnumerable<Member>> validator
        ) : IRequestHandler<CreateBulkMembersCommand>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<IEnumerable<Member>> _validator = validator;

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