using Library.Commands.Member;
using Library.Features.Commands;
using Library.Features.Queries;
using Library.Model;
using Library.Model.Request;
using Library.Repository;
using MediatR;

namespace Library.Service
{
    public class LibraryService(
        ILibraryQueryRepository queryRepo,
        IMediator mediator
        ) : ILibraryService
    {
        private readonly ILibraryQueryRepository _queryRepo = queryRepo;
        private readonly IMediator _mediator = mediator;

        #region Library

        #endregion Library

        #region Book

        public async Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name)
        {
            return await _mediator.Send(new GetBooksQuery(genre, name));
        }

        public async Task<IEnumerable<Book>> GetFullBooksAsync(Genre? genre, string? name)
        {
            return await _mediator.Send(new GetFullBooksQuery(genre, name));
        }

        public async Task<Guid> AddBookAsync(BookRequest request)
        {
            var book = request.Book;
            var qty = request.Qty;
            var newBookId = await _mediator.Send(new AddBookCommand(book, qty));
            return newBookId;
        }

        public async Task<Book> UpdateBookAsync(BookRequest request)
        {
            var updatedBook = await _mediator.Send(new UpdateBookCommand(request.Book));

            var existingBook = (await _mediator.Send(new GetFullBooksQuery(updatedBook.Genre, updatedBook.Name)))
                .FirstOrDefault(b => b.Id == request.Book.Id) ?? throw new InvalidOperationException($"Book with ID {request.Book.Id} not found");

            var bookStock = existingBook.BookStocks.FirstOrDefault(bs => bs.BookId == updatedBook.Id) ?? throw new InvalidOperationException($"BookStock for Book ID {updatedBook.Id} not found");
            bookStock.Quantity = request.Qty;
            await _mediator.Send(new UpdateBookStockCommand(bookStock));

            return updatedBook;
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            await _mediator.Send(new DeleteBookCommand(bookId));
        }

        #endregion Book

        #region Member

        public async Task<IEnumerable<Member>> GetMembersOnlyAsync(string? name, DateOnly? date)
        {
            return await _mediator.Send(new GetMembersQuery(name, date));
        }

        public async Task<IEnumerable<Member>> GetFullMembersAsync(string? name, DateOnly? date)
        {
            return await _mediator.Send(new GetFullMembersQuery(name, date));
        }

        public async Task<Guid> AddMemberAsync(MemberRequest request)
        {
            var member = request.Member;
            var newMemberId = await _mediator.Send(new AddMemberCommand(member));
            return newMemberId;
        }

        public async Task<Member> UpdateMemberAsync(MemberRequest request)
        {
            var updatedMember = await _mediator.Send(new UpdateMemberCommand(request.Member));
            return updatedMember;
        }

        public async Task DeleteMemberAsync(Guid memberId)
        {
            await _mediator.Send(new DeleteMemberCommand(memberId));
        }

        #endregion Member

        #region Loan Book

        public async Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName)
        {
            return await _queryRepo.GetLoanBooksAsync(bookName, memberName);
        }

        public async Task<Guid> AddLoanBookAsync(LoanBookRequest request)
        {
            var loanBook = request.LoanBook;
            var newLoanedBookId = await _mediator.Send(new AddLoanedBookCommand(loanBook));
            return newLoanedBookId;
        }

        public async Task<LoanBook> UpdateLoanedBookReturnedDateAsync(LoanBookRequest request)
        {
            var updatedLoanedBook = await _mediator.Send(new UpdateLoanedBookCommand(request.LoanBook));
            return updatedLoanedBook;
        }

        #endregion Loan Book
    }
}