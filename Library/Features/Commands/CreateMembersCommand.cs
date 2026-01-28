using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class AddMembersCommandHandler(
        ICommandRepo<Member> commandRepo,
        IValidator<IEnumerable<Member>> validator
        ) : IRequestHandler<CreateMembersCommand>
    {
        private readonly ICommandRepo<Member> _commandRepo = commandRepo;
        private readonly IValidator<IEnumerable<Member>> _validator = validator;

        public async Task Handle(CreateMembersCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Members, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _commandRepo.AddRangeAsync(command.Members);
        }
    }
}