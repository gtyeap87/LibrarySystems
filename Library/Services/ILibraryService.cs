using Library.Dto.Requests;
using Library.Models;

namespace Library.Services
{
    public interface ILibraryService
    {
        Task<Guid> CreateBookAsync(BookRequest request);

        Task<Guid> CreateLoanBookAsync(LoanBookRequest request);

        Task<Guid> CreateMemberAsync(MemberRequest request);

        Task DeleteBookAsync(Guid bookId);

        Task DeleteMemberAsync(Guid memberId);

        Task<Book> UpdateBookAsync(BookRequest request);

        Task<LoanBook> UpdateLoanedBookAsync(LoanBookRequest request);

        Task<Member> UpdateMemberAsync(MemberRequest request);

        Task CreateBulkMembersAsync(MembersRequest request);

        Task<IEnumerable<Book>> ReadBooksAsync(Genre? genre, string? name, PaginationRequestDto page);

        Task<IEnumerable<Member>> ReadMembersOnlyAsync(string? name, DateOnly? date, PaginationRequestDto page);

        Task<IEnumerable<Book>> ReadFullBooksAsync(Genre? genre, string? name, PaginationRequestDto page);

        Task<IEnumerable<LoanBook>> ReadLoanBooksAsync(string? bookName, string? memberName, PaginationRequestDto page);

        Task<IEnumerable<Member>> ReadFullMembersAsync(string? name, DateOnly? date, PaginationRequestDto page);

        Task CreateMembersAsync(MembersRequest request);

        Task CreateBooksAsync(BooksRequest request);

        Task CreateBulkBooksAsync(BooksRequest request);

        Task UpdateBulkBooksAsync(BooksRequest request);

        Task UpdateBulkMembersAsync(MembersRequest request);

        Task UpdateBulkLoanedBooksAsync(LoanBooksRequest request);

        Task CreateBulkLoanBooksAsync(LoanBooksRequest request);
    }
}