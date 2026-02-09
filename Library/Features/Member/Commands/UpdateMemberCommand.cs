using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateMemberCommand(Model.Member Member) : IRequest<Model.Member>;

    public class UpdateMemberCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo,
        IValidator<Model.Member> validator
        ) : IRequestHandler<UpdateMemberCommand, Model.Member>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<Model.Member> _validator = validator;

        public async Task<Model.Member> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
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