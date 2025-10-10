namespace Library.Repository.Specification
{
    public interface ISpecification<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}