using System.ComponentModel.DataAnnotations;

namespace Library.Api.Models;

public class Library : Root
{
    public Guid Id { get; set; }
    public required string Location { get; set; }

    public required ICollection<Member> Members { get; set; }
    public required ICollection<Book> Books { get; set; }
}