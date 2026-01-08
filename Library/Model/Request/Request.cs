namespace Library.Model.Request
{
    public class BookRequest
    {
        public required Book Book { get; set; }
        public int Qty { get; set; }
    }

    public class BooksRequest
    {
        public required IEnumerable<BookRequest> Books { get; set; } = [];
    }

    public class MemberRequest
    {
        public required Member Member { get; set; }
    }

    public class MembersRequest
    {
        public required IEnumerable<Member> Members { get; set; }
    }

    public class LoanBookRequest
    {
        public required LoanBook LoanBook { get; set; }
    }
}