using System.ComponentModel.DataAnnotations;

namespace Library.Model;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>
public class Library : Root
{
    public Guid Id { get; set; }

    [Required]
    public required string Location { get; set; }

    public required ICollection<Member> Members { get; set; }
    public required ICollection<Book> Books { get; set; }
}