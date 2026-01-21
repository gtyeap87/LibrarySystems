using Library.Factory;
using System.ComponentModel.DataAnnotations;

namespace Library.Model;

/// <summary>
/// Represents the stock information for a specific book
/// </summary>
public class BookStock : Root, IRoot
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}