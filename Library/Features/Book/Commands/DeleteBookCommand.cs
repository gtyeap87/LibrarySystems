using Library.Dto.Requests;
using Library.Repositories;
using Library.Specifications;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record DeleteBookCommand(Guid BookId, PaginationRequestDto Page) : IRequest;

    public class DeleteBookCommandHandler(
        ICommandRepo<Models.Book> bookCommandRepo,
        IQueryRepo<Models.Book> bookQueryRepo,
        IQueryRepo<Models.LoanBook> loanBookQueryRepo
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