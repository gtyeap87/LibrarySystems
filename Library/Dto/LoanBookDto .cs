using Library.Model;

namespace Library.Dto;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>

public record LoanBookDto(
    string MemberName,
    string BookName,
    DateOnly LoanedDate,
    DateOnly? ReturnedDate
    )
{
    public bool IsBookReturned => ReturnedDate != null;
};

//    /// <summary>
//    /// To get total number of books loaned in one transaction
//    /// </summary>
public record LoanBooksDetailsDto(
    IEnumerable<LoanBookDto> LoanBooks,
    int LoanedOutBooksQuantity
    );