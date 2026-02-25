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
        public async Task<BookStock> Handle(UpdateBookStockCommand command, CancellationToken cancellationToken)
        {
            return await bookStockCommandRepo.UpdateAsync(command.BookStock);
        }
    }
}