using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository.Specification
{
    public class FullLoanedBookSpec(string? bookName, string? memberName) : Specification<LoanBook>
    {
        private readonly string? _bookName = bookName;
        private readonly string? _memberName = memberName;

        public override IQueryable<LoanBook> Apply(IQueryable<LoanBook> query)
        {
            query = query
                .Include(b => b.Book).ThenInclude(bk => bk.BookStocks)
                .Include(b => b.Member);

            if (!string.IsNullOrEmpty(_bookName))
                query = query.Where(m => EF.Functions.Like(m.Book.Name, _bookName));

            if (!string.IsNullOrEmpty(_memberName))
                query = query.Where(m => EF.Functions.Like(m.Member.Name, _memberName));

            return query;
        }
    }
}