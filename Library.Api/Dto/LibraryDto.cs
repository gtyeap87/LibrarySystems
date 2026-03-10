namespace Library.Api.Dto
{
    public record LibraryDto(
        int NoOfMembers,
        int TotalNumbersOfBooks,
        int TotalLoanedBooks
    );
}