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

        Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name);

        Task<IEnumerable<Book>> GetFullBooksAsync(Genre? genre, string? name);

        Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName);

        Task<IEnumerable<Member>> GetMembersExtended2Async(string? name, DateOnly? date);

        Task<IEnumerable<Member>> GetMembersOnlyAsync(string? name, DateOnly? date);

        Task<Book> UpdateBookAsync(BookRequest request);

        Task<LoanBook> UpdateLoanedBookReturnedDateAsync(LoanBookRequest request);

        Task<Member> UpdateMemberAsync(MemberRequest request);
    }
}