using Library.Commands.Member;
using Library.Features.Commands;
using Library.Features.Queries;
using Library.Model;
using Library.Model.Request;
using MediatR;

namespace Library.Service
{
    public class LibraryService(
        IMediator mediator
        ) : ILibraryService
    {
        private readonly IMediator _mediator = mediator;

        #region Library

        #endregion Library

        #region Book

        public async Task<IEnumerable<Book>> ReadBooksAsync(Genre? genre, string? name, PaginationRequest page)
        {
            return await _mediator.Send(new ReadBooksQuery(genre, name, page));
        }

        public async Task<IEnumerable<Book>> ReadFullBooksAsync(Genre? genre, string? name, PaginationRequest page)
        {
            return await _mediator.Send(new ReadFullBooksQuery(genre, name, page));
        }

        public async Task<Guid> CreateBookAsync(BookRequest request)
        {
            var book = request.Book;
            var qty = request.Qty;
            var newBookId = await _mediator.Send(new CreateBookCommand(book, qty));
            return newBookId;
        }

        public async Task CreateBooksAsync(BooksRequest request)
        {
            var bookRequests = request.Books;
            var books = bookRequests.Select(br => (br.Book, br.Qty));
            await _mediator.Send(new CreateBooksCommand(books));
        }

        public async Task CreateBulkBooksAsync(BooksRequest request)
        {
            var bookRequests = request.Books;
            var books = bookRequests.Select(br => (br.Book, br.Qty));
            await _mediator.Send(new CreateBulkBooksCommand(books));
        }

        public async Task<Book> UpdateBookAsync(BookRequest request)
        {
            //todo: optimze this update by utilizing getbyidaysnc in command handler
            var updatedBook = await _mediator.Send(new UpdateBookCommand(request.Book));

            var existingBook = (await _mediator.Send(new ReadFullBooksQuery(updatedBook.Genre, updatedBook.Name,
                new PaginationRequest()
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue
                })))
                .FirstOrDefault(b => b.Id == request.Book.Id) ?? throw new InvalidOperationException($"Book with ID {request.Book.Id} not found");

            var bookStock = existingBook.BookStocks.FirstOrDefault(bs => bs.BookId == updatedBook.Id) ?? throw new InvalidOperationException($"BookStock for Book ID {updatedBook.Id} not found");
            bookStock.Quantity = request.Qty;
            await _mediator.Send(new UpdateBookStockCommand(bookStock));

            return updatedBook;
        }

        public async Task UpdateBulkBooksAsync(BooksRequest request)
        {
            var books = request.Books.Select(br => (br.Book, br.Qty));
            await _mediator.Send(new UpdateBulkBooksCommand(books));
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            await _mediator.Send(new DeleteBookCommand(bookId, new PaginationRequest() { PageNumber = 1, PageSize = int.MaxValue }));
        }

        #endregion Book

        #region Member

        public async Task<IEnumerable<Member>> ReadMembersOnlyAsync(string? name, DateOnly? date, PaginationRequest page)
        {
            return await _mediator.Send(new ReadMembersQuery(name, date, page));
        }

        public async Task<IEnumerable<Member>> ReadFullMembersAsync(string? name, DateOnly? date, PaginationRequest page)
        {
            return await _mediator.Send(new ReadFullMembersQuery(name, date, page));
        }

        public async Task<Guid> CreateMemberAsync(MemberRequest request)
        {
            var member = request.Member;
            var newMemberId = await _mediator.Send(new CreateMemberCommand(member));
            return newMemberId;
        }

        public async Task CreateBulkMembersAsync(MembersRequest request)
        {
            var members = request.Members;
            await _mediator.Send(new CreateBulkMembersCommand(members));
        }

        public async Task CreateMembersAsync(MembersRequest request)
        {
            var members = request.Members;
            await _mediator.Send(new CreateMembersCommand(members));
        }

        public async Task<Member> UpdateMemberAsync(MemberRequest request)
        {
            var updatedMember = await _mediator.Send(new UpdateMemberCommand(request.Member));
            return updatedMember;
        }

        public async Task UpdateBulkMembersAsync(MembersRequest request)
        {
            var members = request.Members;
            await _mediator.Send(new UpdateBulkMembersCommand(members));
        }

        public async Task DeleteMemberAsync(Guid memberId)
        {
            await _mediator.Send(new DeleteMemberCommand(memberId, new PaginationRequest() { PageSize = int.MaxValue }));
        }

        #endregion Member

        #region Loan Book

        public async Task<IEnumerable<LoanBook>> ReadLoanBooksAsync(string? bookName, string? memberName, PaginationRequest page)
        {
            return await _mediator.Send(new ReadFullLoanedBooksQuery(bookName, memberName, page));
        }

        public async Task<Guid> CreateLoanBookAsync(LoanBookRequest request)
        {
            var loanBook = request.LoanBook;
            var newLoanedBookId = await _mediator.Send(new CreateLoanedBookCommand(loanBook));
            return newLoanedBookId;
        }

        public async Task<LoanBook> UpdateLoanedBookAsync(LoanBookRequest request)
        {
            var updatedLoanedBook = await _mediator.Send(new UpdateLoanedBookCommand(request.LoanBook));
            return updatedLoanedBook;
        }

        public async Task UpdateBulkLoanedBooksAsync(LoanBooksRequest request)
        {
            var loanBooks = request.LoanBooks;
            await _mediator.Send(new UpdateBulkLoanBooksCommand(loanBooks));
        }

        #endregion Loan Book
    }
}