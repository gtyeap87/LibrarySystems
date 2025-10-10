using Library.Data;
using Library.Model;
using Library.Repository.Specification;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository;

/// <summary>
/// Repository for managing library data with logging capabilities
/// </summary>
public class LibraryRepository(
    LibraryContext context,
    ILogger<LibraryRepository> logger) : ILibraryQueryRepository, ILibraryCommandRepository
{
    private readonly LibraryContext _context = context;
    private readonly ILogger<LibraryRepository> _logger = logger;

    #region Book

    public async Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name)
    {
        _logger.LogInformation("Retrieving books based on genre or name");

        var books = await _context.Books
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
        _logger.LogInformation("Adding a new book to the library");

        _context.Books.Add(book);
        _context.BookStocks.Add(new BookStock
        {
            BookId = book.Id,
            Quantity = qty
        });

        await _context.SaveChangesAsync();

        return book.Id;
    }

    //TODO - Bulk insert with batches

    public async Task<Book> UpdateBookAsync(Book book)
    {
        _logger.LogInformation("Updating book information");
        var existingForecast = await _context.Books
            .FirstOrDefaultAsync(f => f.Id == book.Id);

        if (existingForecast == null)
        {
            _logger.LogWarning("Book with ID: {Id} not found for update", book.Id);
            throw new KeyNotFoundException($"Book with ID {book.Id} not found.");
        }

        _context.Entry(existingForecast).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully updated weather forecast with ID: {Id}", book.Id);
        return existingForecast;
    }

    #endregion Book

    #region Member

    public async Task<IEnumerable<Member>> GetMembersAsync(string? name, DateOnly? date, bool include = false)
    {
        _logger.LogInformation("Retrieving members based on name or date");

        if (include)
        {
            var members = await _context.Members
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
            var members = await _context.Members
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
        _logger.LogInformation("Adding a new member to the library");

        _context.Members.Add(member);

        await _context.SaveChangesAsync();

        return member.Id;
    }

    #endregion Member

    #region Loan Book

    public async Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName)
    {
        _logger.LogInformation("Retrieving loaned books based on book name or member name");
        var books = await _context.LoanBooks
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
        _logger.LogInformation("Adding a new loaned book record");

        _context.LoanBooks.Add(loanBook);

        await _context.SaveChangesAsync();

        return loanBook.Id;
    }

    #endregion Loan Book

    ///// <inheritdoc/>
    //public async Task<IEnumerable<Member>> GetAllAsync()
    //{
    //    _logger.LogInformation("Retrieving all weather forecasts");

    //    var forecasts = await _context.Books.ToListAsync();
    //    _logger.LogInformation("Retrieved {Count} weather forecasts", forecasts.Count);
    //    return forecasts;
    //}

    ///// <inheritdoc/>
    //public async Task<Book?> GetByIdAsync(int id)
    //{
    //    _logger.LogInformation("Retrieving weather forecast with ID: {Id}", id);

    //    var forecast = await _context.Books.FindAsync(id);
    //    if (forecast == null)
    //    {
    //        _logger.LogWarning("Weather forecast with ID: {Id} not found", id);
    //    }
    //    return forecast;
    //}

    ///// <inheritdoc/>
    //public async Task<Book> UpdateAsync(Book forecast)
    //{
    //    _logger.LogInformation("Updating weather forecast with ID: {Id}", forecast.Id);

    //    var existingForecast = await _context.Books
    //        .FirstOrDefaultAsync(f => f.Id == forecast.Id);

    //    if (existingForecast == null)
    //    {
    //        _logger.LogWarning("Weather forecast with ID: {Id} not found for update", forecast.Id);
    //        throw new KeyNotFoundException($"Weather forecast with ID {forecast.Id} not found.");
    //    }

    //    _context.Entry(existingForecast).CurrentValues.SetValues(forecast);
    //    await _context.SaveChangesAsync();

    //    _logger.LogInformation("Successfully updated weather forecast with ID: {Id}", forecast.Id);
    //    return existingForecast;
    //}
}