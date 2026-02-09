using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMemberCommand(Model.Member Member) : IRequest<Guid>;

    public class AddMemberCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo,
        IValidator<Model.Member> validator
        ) : IRequestHandler<CreateMemberCommand, Guid>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<Model.Member> _validator = validator;

        public async Task<Guid> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Member, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var newId = await _memberCommandRepo.AddAsync(command.Member);
            return newId;
        }
    }
}