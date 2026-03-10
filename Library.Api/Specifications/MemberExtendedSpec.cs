using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Specifications
{
    public class MembersWithLoansSpec(string? name, DateOnly? date) : MembersOnlySpec(name, date)
    {
        public override IQueryable<Member> Apply(IQueryable<Member> query)
        {
            // Start with base filters
            query = base.Apply(query);

            // Add include
            query = query.Include(m => m.LoanedBooks)
                         .ThenInclude(lb => lb.Book);

            return query;
        }
    }
}