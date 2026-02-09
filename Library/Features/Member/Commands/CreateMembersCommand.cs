using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

    public class AddMembersCommandHandler(
        ICommandRepo<Model.Member> commandRepo,
        IValidator<IEnumerable<Model.Member>> validator
        ) : IRequestHandler<CreateMembersCommand>
    {
        private readonly ICommandRepo<Model.Member> _commandRepo = commandRepo;
        private readonly IValidator<IEnumerable<Model.Member>> _validator = validator;

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