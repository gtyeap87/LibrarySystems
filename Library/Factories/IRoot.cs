namespace Library.Factories
{
    /// <summary>
    /// Used to assign common properties to root entities in bulk scenarios
    /// </summary>
    public interface IRoot
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
        byte[]? RowVersion { get; set; }
    }
}