using Library.Factory;
using System.ComponentModel.DataAnnotations;

namespace Library.Model;

/// <summary>
/// Represents available book data in the library system
/// </summary>
public class Book : Root, IRoot
{
    public Guid Id { get; set; }
    public Guid LibraryId { get; set; }
    public ICollection<BookStock> BookStocks { get; set; } = [];
    public required Genre Genre { get; set; }
    public required string Name { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}

public enum Genre
{
    Unknown,
    Fiction,
    NonFiction,
    Science,
    History,
    Biography,
    Fantasy,
    Mystery,
    Romance,
    Thriller,
    SelfHelp,
    Health,
    Travel,
    Children,
    YoungAdult,
    Religion
}