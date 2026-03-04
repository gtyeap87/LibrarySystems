namespace Library.Specifications
{
    public interface ISpecification<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}