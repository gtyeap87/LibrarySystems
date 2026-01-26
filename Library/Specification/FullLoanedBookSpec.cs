using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Library.Specification
{
    public class FullLoanedBookSpec(string? bookName, string? memberName) : Specification<LoanBook>
    {
        public override IQueryable<LoanBook> Apply(IQueryable<LoanBook> query)
        {
            query = query
                .Include(b => b.Book).ThenInclude(bk => bk.BookStocks)
                .Include(b => b.Member);

            if (!string.IsNullOrEmpty(bookName))
            {
                var pattern = $"%{bookName.Trim()}%";
                query = query.Where(m => EF.Functions.Like(m.Book.Name, pattern));
            }

            if (!string.IsNullOrEmpty(memberName))
            {
                var pattern = $"%{memberName.Trim()}%";
                query = query.Where(m => EF.Functions.Like(m.Member.Name, pattern));
            }

            query = query.OrderBy(b => b.Id);

            return query;
        }
    }
}