using Library.Model;
using Library.Model.Request;

namespace Library.Service
{
    public interface ILibraryService
    {
        Task<Guid> AddBookAsync(BookRequest request);

        Task<Guid> AddLoanBookAsync(LoanBookRequest request);

        Task<Guid> AddMemberAsync(MemberRequest request);

        Task DeleteBookAsync(Guid bookId);

        Task DeleteMemberAsync(Guid memberId);

        Task<Book> UpdateBookAsync(BookRequest request);

        Task<LoanBook> UpdateLoanedBookReturnedDateAsync(LoanBookRequest request);

        Task<Member> UpdateMemberAsync(MemberRequest request);

        Task AddBulkMemberAsync(MembersRequest request);

        Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name, PaginationRequest page);

        Task<IEnumerable<Member>> GetMembersOnlyAsync(string? name, DateOnly? date, PaginationRequest page);

        Task<IEnumerable<Book>> GetFullBooksAsync(Genre? genre, string? name, PaginationRequest page);

        Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName, PaginationRequest page);

        Task<IEnumerable<Member>> GetFullMembersAsync(string? name, DateOnly? date, PaginationRequest page);

        Task AddMembersAsync(MembersRequest request);

        Task AddBooksAsync(BooksRequest request);
    }
}