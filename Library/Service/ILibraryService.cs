using Library.Model;
using Library.Model.Request;

namespace Library.Service
{
    public interface ILibraryService
    {
        Task<Guid> CreateBookAsync(BookRequest request);

        Task<Guid> CreateLoanBookAsync(LoanBookRequest request);

        Task<Guid> CreateMemberAsync(MemberRequest request);

        Task DeleteBookAsync(Guid bookId);

        Task DeleteMemberAsync(Guid memberId);

        Task<Book> UpdateBookAsync(BookRequest request);

        Task<LoanBook> UpdateLoanedBookReturnedDateAsync(LoanBookRequest request);

        Task<Member> UpdateMemberAsync(MemberRequest request);

        Task CreateBulkMembersAsync(MembersRequest request);

        Task<IEnumerable<Book>> ReadBooksAsync(Genre? genre, string? name, PaginationRequest page);

        Task<IEnumerable<Member>> ReadMembersOnlyAsync(string? name, DateOnly? date, PaginationRequest page);

        Task<IEnumerable<Book>> ReadFullBooksAsync(Genre? genre, string? name, PaginationRequest page);

        Task<IEnumerable<LoanBook>> ReadLoanBooksAsync(string? bookName, string? memberName, PaginationRequest page);

        Task<IEnumerable<Member>> ReadFullMembersAsync(string? name, DateOnly? date, PaginationRequest page);

        Task CreateMembersAsync(MembersRequest request);

        Task CreateBooksAsync(BooksRequest request);

        Task CreateBulkBooksAsync(BooksRequest request);
    }
}