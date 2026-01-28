using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateMemberCommand(Member Member) : IRequest<Member>;

    public class UpdateMemberCommandHandler(
        ICommandRepo<Member> memberCommandRepo,
        IValidator<Member> validator
        ) : IRequestHandler<UpdateMemberCommand, Member>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<Member> _validator = validator;

        public async Task<Member> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command.Member, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await _memberCommandRepo.UpdateAsync(command.Member);
        }
    }
}