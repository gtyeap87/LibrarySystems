using Library.Model;

namespace Library.Dto;

/// <summary>
/// Represents Book data with support for both v1 and v2 API features
/// </summary>
public class BookDto
{
    public Genre Genre { get; set; }
    public required string Name { get; set; }
    public int AvailableQuantity { get; set; }
}