using FluentValidation;
using Library.Dto;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateMemberCommand(Member Member) : IRequest<Guid>;

    public class AddMemberCommandHandler(
        ICommandRepo<Member> memberCommandRepo,
        IValidator<Member> validator
        ) : IRequestHandler<CreateMemberCommand, Guid>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<Member> _validator = validator;

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