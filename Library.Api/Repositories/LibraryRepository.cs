using Library.Api.Data.Contexts;
using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Repositories;

/// <summary>
/// Repository for managing library data with logging capabilities
/// </summary>
[Obsolete(message: "this repository class has been deprecated, please use spec class")]
public class LibraryRepository(
    LibraryContext context,
    ILogger<LibraryRepository> logger) : ILibraryQueryRepository, ILibraryCommandRepository
{
    #region Book

    public async Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name)
    {
        logger.LogInformation("Retrieving books based on genre or name");

        var books = await context.Books
            .Include(b => b.BookStocks)
            .Where(b =>
                (!genre.HasValue || b.Genre == genre.Value) &&
                (string.IsNullOrEmpty(name) || b.Name.ToLower() == name.ToLower()))
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

        return books;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="book"></param>
    /// <param name="qty"></param>
    /// <returns></returns>
    public async Task<Guid> AddBookAsync(Book book, int qty)
    {
        logger.LogInformation("Adding a new book to the library");

        context.Books.Add(book);
        context.BookStocks.Add(new BookStock
        {
            BookId = book.Id,
            Quantity = qty
        });

        await context.SaveChangesAsync();

        return book.Id;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        logger.LogInformation("Updating book information");
        var existingForecast = await context.Books
            .FirstOrDefaultAsync(f => f.Id == book.Id);

        if (existingForecast == null)
        {
            logger.LogWarning("Book with ID: {Id} not found for update", book.Id);
            throw new KeyNotFoundException($"Book with ID {book.Id} not found.");
        }

        context.Entry(existingForecast).CurrentValues.SetValues(book);
        await context.SaveChangesAsync();

        logger.LogInformation("Successfully updated weather forecast with ID: {Id}", book.Id);
        return existingForecast;
    }

    #endregion Book

    #region Member

    public async Task<IEnumerable<Member>> GetMembersAsync(string? name, DateOnly? date, bool include = false)
    {
        logger.LogInformation("Retrieving members based on name or date");

        if (include)
        {
            var members = await context.Members
           .Include(b => b.LoanedBooks).ThenInclude(lb => lb.Book)
           .Where(b =>
               (!date.HasValue || b.JoinedDate == date) &&
               (string.IsNullOrEmpty(name) || b.Name.ToLower() == name.ToLower()))
           .AsNoTrackingWithIdentityResolution()
           .ToListAsync();

            return members;
        }
        else
        {
            var members = await context.Members
           .Where(b =>
               (!date.HasValue || b.JoinedDate == date) &&
               (string.IsNullOrEmpty(name) || b.Name.ToLower() == name.ToLower()))
           .AsNoTracking()
           .ToListAsync();
            return members;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="member"></param>
    /// <param name="qty"></param>
    /// <returns></returns>
    public async Task<Guid> AddMemberAsync(Member member)
    {
        logger.LogInformation("Adding a new member to the library");

        context.Members.Add(member);

        await context.SaveChangesAsync();

        return member.Id;
    }

    #endregion Member

    #region Loan Book

    public async Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName)
    {
        logger.LogInformation("Retrieving loaned books based on book name or member name");
        var books = await context.LoanBooks
            .Include(b => b.Book).ThenInclude(bk => bk.BookStocks)
            .Include(b => b.Member)
            .Where(b =>
                (string.IsNullOrEmpty(bookName) || b.Book.Name.ToLower() == bookName.ToLower()) &&
                (string.IsNullOrEmpty(memberName) || b.Member.Name.ToLower() == memberName.ToLower()))
            .AsNoTracking()
            .ToListAsync();

        return books;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="loanBook"></param>
    /// <returns></returns>
    public async Task<Guid> AddLoanBookAsync(LoanBook loanBook)
    {
        logger.LogInformation("Adding a new loaned book record");

        context.LoanBooks.Add(loanBook);

        await context.SaveChangesAsync();

        return loanBook.Id;
    }

    #endregion Loan Book
}