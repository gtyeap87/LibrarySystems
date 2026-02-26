using Library.Dto.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record DeleteBookCommand(Guid BookId, PaginationRequestDto Page) : IRequest;

    public class DeleteBookCommandHandler(
        ICommandRepo<Model.Book> bookCommandRepo,
        IQueryRepo<Model.Book> bookQueryRepo,
        IQueryRepo<Model.LoanBook> loanBookQueryRepo
        ) : IRequestHandler<DeleteBookCommand>
    {
        public async Task Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var book = await bookQueryRepo.GetByIdAsync(command.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {command.BookId} not found.");

            var spec = new FullLoanedBookSpec(book.Name, null);
            var activeLoans = await loanBookQueryRepo.ListAsync(spec, command.Page);

            if (activeLoans.Any())
            {
                throw new InvalidOperationException("Cannot delete book while book still under loan.");
            }

            await bookCommandRepo.DeleteAsync(book);
        }
    }
}