using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Specifications
{
    public class FullLoanedBookSpec(string? bookName, string? memberName) : Specification<LoanBook>
    {
        public override IQueryable<LoanBook> Apply(IQueryable<LoanBook> query)
        {
            query = query
                .Include(b => b.Book)
                .Include(b => b.Member);

            if (!string.IsNullOrEmpty(bookName))
            {
                var pattern = $"%{bookName.Trim()}%";
                query = query.Where(lb => EF.Functions.Like(lb.Book.Name, pattern));
            }

            if (!string.IsNullOrEmpty(memberName))
            {
                var pattern = $"%{memberName.Trim()}%";
                query = query.Where(lb => EF.Functions.Like(lb.Member.Name, pattern));
            }

            query = query.OrderBy(b => b.Id);

            return query;
        }
    }
}