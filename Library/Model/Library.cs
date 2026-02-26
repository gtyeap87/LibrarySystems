using System.ComponentModel.DataAnnotations;

namespace Library.Model;

public class Library : Root
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string Location { get; set; }

    public required ICollection<Member> Members { get; set; }
    public required ICollection<Book> Books { get; set; }
}