using Library.Models;

namespace Library.Dto;

/// <summary>
/// Represents Book data with support for both v1 and v2 API features
/// </summary>
public record BookDto(
    Genre Genre,
    string Name,
    int AvailableQuantity
    );