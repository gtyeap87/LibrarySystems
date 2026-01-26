using Library.Model;

namespace Library.Specification
{
    public class MembersOnlySpec(string? name, DateOnly? date) : Specification<Member>
    {
        private readonly string? _name = name;
        private readonly DateOnly? _date = date;

        public override IQueryable<Member> Apply(IQueryable<Member> query)
        {
            if (_date.HasValue)
                query = query.Where(m => m.JoinedDate == _date.Value);

            if (!string.IsNullOrEmpty(_name))
                query = query.Where(m => m.Name.ToLower() == _name.ToLower());

            return query;
        }
    }
}