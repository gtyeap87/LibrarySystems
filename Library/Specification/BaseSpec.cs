namespace Library.Specification
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract IQueryable<T> Apply(IQueryable<T> query);
    }
}