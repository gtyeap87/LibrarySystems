using Library.Dto.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record DeleteBookCommand(Guid BookId, PaginationRequestDto Page) : IRequest;

    public class DeleteBookCommandHandler(
        ICommandRepo<Model.Book> memberCommandRepo,
        IQueryRepo<Model.Book> memberQueryRepo,
        IQueryRepo<Model.LoanBook> loanBookQueryRepo
        ) : IRequestHandler<DeleteBookCommand>
    {
        public async Task Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var member = await memberQueryRepo.GetByIdAsync(command.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {command.BookId} not found.");

            var spec = new FullLoanedBookSpec(null, member.Name);
            var activeLoans = await loanBookQueryRepo.ListAsync(spec, command.Page);

            if (activeLoans.Any())
            {
                throw new InvalidOperationException("Cannot delete member with active loans.");
            }

            await memberCommandRepo.DeleteAsync(member);
        }
    }
}