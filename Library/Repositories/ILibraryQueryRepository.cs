using Library.Models;

namespace Library.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface ILibraryQueryRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name);

        Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName);

        Task<IEnumerable<Member>> GetMembersAsync(string? name, DateOnly? date, bool include = false);
    }
}