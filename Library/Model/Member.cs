using System.ComponentModel.DataAnnotations;

namespace Library.Model;

/// <summary>
/// Represents member data in the library system
/// </summary>
public class Member : Root
{
    public Guid Id { get; set; }
    public Guid LibraryId { get; set; }
    public ICollection<LoanBook> LoanedBooks { get; set; } = [];
    public required string Name { get; set; }
    public DateOnly JoinedDate { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}