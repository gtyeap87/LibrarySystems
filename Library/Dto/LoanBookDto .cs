using Library.Model;

namespace Library.Dto;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>
public class LoanBookDto
{
    public required string MemberName { get; set; }
    public required string BookName { get; set; }
    public DateOnly LoanedDate { get; set; }
    public DateOnly? ReturnedDate { get; set; }

    public bool IsBookReturned
    {
        get => ReturnedDate != null;
    }
}

public class LoanBooksDetailsDto
{
    public required IEnumerable<LoanBookDto> LoanBooks { get; set; }

    /// <summary>
    /// To get total number of books loaned in one transaction
    /// </summary>
    public int LoanBookQuantity { get; set; }
}