using Library.Commands.Member;
using Library.Dto.Request;
using Library.Features.Commands;
using Library.Features.Queries;
using Library.Model;
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

        public async Task<IEnumerable<Book>> ReadBooksAsync(Genre? genre, string? name, PaginationRequestDto page)
        {
            return await _mediator.Send(new ReadBooksQuery(genre, name, page));
        }

        public async Task<IEnumerable<Book>> ReadFullBooksAsync(Genre? genre, string? name, PaginationRequestDto page)
        {
            return await _mediator.Send(new ReadFullBooksQuery(genre, name, page));
        }

        public async Task<Guid> CreateBookAsync(BookRequest request)
        {
            var newBookId = await _mediator.Send(new CreateBookCommand(request));
            return newBookId;
        }

        public async Task CreateBooksAsync(BooksRequest request)
        {
            await _mediator.Send(new CreateBooksCommand(request));
        }

        public async Task CreateBulkBooksAsync(BooksRequest request)
        {
            await _mediator.Send(new CreateBulkBooksCommand(request));
        }

        public async Task<Book> UpdateBookAsync(BookRequest request)
        {
            var updatedBook = await _mediator.Send(new UpdateBookCommand(request));

            var existingBook = (await _mediator.Send(new ReadFullBooksQuery(updatedBook.Genre, updatedBook.Name,
                new PaginationRequestDto()
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue
                })))
                .FirstOrDefault(b => b.Id == request.Book.Id)
                ?? throw new InvalidOperationException($"Book with ID {request.Book.Id} not found");

            var bookStock = existingBook.BookStocks.FirstOrDefault(bs => bs.BookId == updatedBook.Id)
                ?? throw new InvalidOperationException($"BookStock for Book ID {updatedBook.Id} not found");

            bookStock.Quantity = request.Qty;
            await _mediator.Send(new UpdateBookStockCommand(bookStock));

            return updatedBook;
        }

        public async Task UpdateBulkBooksAsync(BooksRequest request)
        {
            await _mediator.Send(new UpdateBulkBooksCommand(request));
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            await _mediator.Send(new DeleteBookCommand(bookId, new PaginationRequestDto() { PageNumber = 1, PageSize = int.MaxValue }));
        }

        #endregion Book

        #region Member

        public async Task<IEnumerable<Member>> ReadMembersOnlyAsync(string? name, DateOnly? date, PaginationRequestDto page)
        {
            return await _mediator.Send(new ReadMembersQuery(name, date, page));
        }

        public async Task<IEnumerable<Member>> ReadFullMembersAsync(string? name, DateOnly? date, PaginationRequestDto page)
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
            var members = request.Members.Select(x => x.Member);
            await _mediator.Send(new CreateBulkMembersCommand(members));
        }

        public async Task CreateMembersAsync(MembersRequest request)
        {
            var members = request.Members.Select(x => x.Member);
            await _mediator.Send(new CreateMembersCommand(members));
        }

        public async Task<Member> UpdateMemberAsync(MemberRequest request)
        {
            var updatedMember = await _mediator.Send(new UpdateMemberCommand(request.Member));
            return updatedMember;
        }

        public async Task UpdateBulkMembersAsync(MembersRequest request)
        {
            var members = request.Members.Select(x => x.Member);
            await _mediator.Send(new UpdateBulkMembersCommand(members));
        }

        public async Task DeleteMemberAsync(Guid memberId)
        {
            await _mediator.Send(new DeleteMemberCommand(memberId, new PaginationRequestDto() { PageSize = int.MaxValue }));
        }

        #endregion Member

        #region Loan Book

        public async Task<IEnumerable<LoanBook>> ReadLoanBooksAsync(string? bookName, string? memberName, PaginationRequestDto page)
        {
            return await _mediator.Send(new ReadFullLoanedBooksQuery(bookName, memberName, page));
        }

        public async Task<Guid> CreateLoanBookAsync(LoanBookRequest request)
        {
            var loanBook = request.LoanBook;
            var newLoanedBookId = await _mediator.Send(new CreateLoanedBookCommand(loanBook));
            return newLoanedBookId;
        }

        public async Task CreateBulkLoanBooksAsync(LoanBooksRequest request)
        {
            var loanBooks = request.LoanBooks;
            await _mediator.Send(new CreateBulkLoanBooksCommand(loanBooks));
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