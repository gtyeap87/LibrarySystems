using System.ComponentModel.DataAnnotations;

namespace Library.Model;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>
public class BookStock : Root
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}