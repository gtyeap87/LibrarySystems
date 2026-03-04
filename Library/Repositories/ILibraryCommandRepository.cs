using Library.Models;

namespace Library.Repositories;

/// <summary>
///
/// </summary>
public interface ILibraryCommandRepository
{
    Task<Guid> AddBookAsync(Book book, int qty);

    Task<Guid> AddLoanBookAsync(LoanBook loanBook);

    Task<Guid> AddMemberAsync(Member member);

    Task<Book> UpdateBookAsync(Book book);
}