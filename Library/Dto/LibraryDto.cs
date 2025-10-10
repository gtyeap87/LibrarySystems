namespace Library.Dto
{
    public record LibraryDto(
        int NoOfMembers,
        int TotalNumbersOfBooks,
        int TotalLoanedBooks
    );
}