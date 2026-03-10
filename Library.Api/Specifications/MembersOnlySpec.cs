using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Specifications
{
    public class MembersOnlySpec(string? name, DateOnly? date) : Specification<Member>
    {
        public override IQueryable<Member> Apply(IQueryable<Member> query)
        {
            if (date.HasValue)
                query = query.Where(m => m.JoinedDate == date.Value);

            if (!string.IsNullOrEmpty(name))
            {
                var pattern = $"%{name.Trim()}%";
                query = query.Where(m => EF.Functions.Like(m.Name, pattern));
            }

            query = query.OrderBy(m => m.Id);

            return query;
        }
    }
}