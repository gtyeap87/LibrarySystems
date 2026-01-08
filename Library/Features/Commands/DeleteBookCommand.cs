using Library.Model;
using Library.Model.Request;
using Library.Repository;
using Library.Repository.Specification;
using MediatR;

namespace Library.Features.Commands
{
    public record DeleteBookCommand(Guid BookId, PaginationRequest Page) : IRequest;

    public class DeleteBookCommandHandler(
        ICommandRepo<Book> memberCommandRepo,
        IQueryRepo<Book> memberQueryRepo,
        IQueryRepo<LoanBook> loanBookQueryRepo
        ) : IRequestHandler<DeleteBookCommand>
    {
        private readonly ICommandRepo<Book> _memberCommandRepo = memberCommandRepo;
        private readonly IQueryRepo<Book> _memberQueryRepo = memberQueryRepo;
        private readonly IQueryRepo<LoanBook> _loanBookQueryRepo = loanBookQueryRepo;

        public async Task Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var member = await _memberQueryRepo.GetByIdAsync(command.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {command.BookId} not found.");

            var spec = new FullLoanedBookSpec(null, member.Name);
            var activeLoans = await _loanBookQueryRepo.ListAsync(spec, command.Page);

            if (activeLoans.Any())
            {
                throw new InvalidOperationException("Cannot delete member with active loans.");
            }

            await _memberCommandRepo.DeleteAsync(member);
        }
    }
}