using Library.Repository;
using MediatR;

namespace Library.Commands.Member
{
    public record UpdateMemberCommand(Model.Member Member) : IRequest<Model.Member>;

    public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Model.Member>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo;

        public UpdateMemberCommandHandler(ICommandRepo<Model.Member> memberCommandRepo)
        {
            _memberCommandRepo = memberCommandRepo;
        }

        public async Task<Model.Member> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.UpdateAsync(command.Member);
        }
    }
}