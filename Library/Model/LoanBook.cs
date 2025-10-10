using System.ComponentModel.DataAnnotations;

namespace Library.Model;

/// <summary>
/// Represents Loan book where it keeps track member who borrows which book
/// </summary>
public class LoanBook : Root
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public Guid MemberId { get; set; }

    public Member? Member { get; set; }
    public DateOnly LoanedDate { get; set; }

    public DateOnly? ReturnedDate { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}