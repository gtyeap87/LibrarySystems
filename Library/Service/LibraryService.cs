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
        IMediator mediator
        ) : ILibraryService
    {
        private readonly IMediator _mediator = mediator;

        #region Library

        #endregion Library

        #region Book

        //public async Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name, PaginationRequest page)
        //{
        //    return await _mediator.Send(new GetBooksQuery(genre, name, page));
        //}

        //public async Task<IEnumerable<Book>> GetFullBooksAsync(Genre? genre, string? name, PaginationRequest page)
        //{
        //    return await _mediator.Send(new GetFullBooksQuery(genre, name, page));
        //}

        public async Task<Guid> AddBookAsync(BookRequest request)
        {
            var book = request.Book;
            var qty = request.Qty;
            var newBookId = await _mediator.Send(new AddBookCommand(book, qty));
            return newBookId;
        }

        public async Task AddBooksAsync(BooksRequest request)
        {
            var bookRequests = request.Books;
            var books = bookRequests.Select(br => (br.Book, br.Qty));
            await _mediator.Send(new AddBooksCommand(books));
        }

        //public async Task<Book> UpdateBookAsync(BookRequest request)
        //{
        //    var updatedBook = await _mediator.Send(new UpdateBookCommand(request.Book));

        //    var existingBook = (await _mediator.Send(new GetFullBooksQuery(updatedBook.Genre, updatedBook.Name,
        //        new PaginationRequest()
        //        {
        //            PageNumber = 1,
        //            PageSize = int.MaxValue
        //        })))
        //        .FirstOrDefault(b => b.Id == request.Book.Id) ?? throw new InvalidOperationException($"Book with ID {request.Book.Id} not found");

        //    var bookStock = existingBook.BookStocks.FirstOrDefault(bs => bs.BookId == updatedBook.Id) ?? throw new InvalidOperationException($"BookStock for Book ID {updatedBook.Id} not found");
        //    bookStock.Quantity = request.Qty;
        //    await _mediator.Send(new UpdateBookStockCommand(bookStock));

        //    return updatedBook;
        //}

        //public async Task DeleteBookAsync(Guid bookId)
        //{
        //    await _mediator.Send(new DeleteBookCommand(bookId, new PaginationRequest() { PageNumber = 1, PageSize = int.MaxValue }));
        //}

        #endregion Book

        #region Member

        //public async Task<IEnumerable<Member>> GetMembersOnlyAsync(string? name, DateOnly? date, PaginationRequest page)
        //{
        //    return await _mediator.Send(new GetMembersQuery(name, date, page));
        //}

        //public async Task<IEnumerable<Member>> GetFullMembersAsync(string? name, DateOnly? date, PaginationRequest page)
        //{
        //    return await _mediator.Send(new GetFullMembersQuery(name, date, page));
        //}

        public async Task<Guid> AddMemberAsync(MemberRequest request)
        {
            var member = request.Member;
            var newMemberId = await _mediator.Send(new AddMemberCommand(member));
            return newMemberId;
        }

        public async Task AddBulkMemberAsync(MembersRequest request)
        {
            throw new NotImplementedException($"Method {nameof(AddBulkMemberAsync)} is not yet implemented.");
        }

        public async Task AddMembersAsync(MembersRequest request)
        {
            var members = request.Members;
            await _mediator.Send(new AddMembersCommand(members));
        }

        public async Task<Member> UpdateMemberAsync(MemberRequest request)
        {
            var updatedMember = await _mediator.Send(new UpdateMemberCommand(request.Member));
            return updatedMember;
        }

        //public async Task DeleteMemberAsync(Guid memberId)
        //{
        //    await _mediator.Send(new DeleteMemberCommand(memberId, new PaginationRequest() { PageSize = int.MaxValue }));
        //}

        #endregion Member

        #region Loan Book

        //public async Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName, PaginationRequest page)
        //{
        //    return await _mediator.Send(new GetFullLoanedBooksQuery(bookName, memberName, page));
        //}

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