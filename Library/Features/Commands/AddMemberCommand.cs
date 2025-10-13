using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record AddMemberCommand(Member Member) : IRequest<Guid>;

    public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Guid>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo;

        public AddMemberCommandHandler(ICommandRepo<Member> memberCommandRepo)
        {
            _memberCommandRepo = memberCommandRepo;
        }

        public async Task<Guid> Handle(AddMemberCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.AddAsync(command.Member);
        }
    }
}