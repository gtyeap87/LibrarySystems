using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record UpdateBookStockCommand(BookStock BookStock) : IRequest<BookStock>;

    public class UpdateBookStockCommandHandler(
        ICommandRepo<BookStock> bookStockCommandRepo
        ) : IRequestHandler<UpdateBookStockCommand, BookStock>
    {
        private readonly ICommandRepo<BookStock> _memberCommandRepo = bookStockCommandRepo;

        public async Task<BookStock> Handle(UpdateBookStockCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.UpdateAsync(command.BookStock);
        }
    }
}