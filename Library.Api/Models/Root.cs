namespace Library.Api.Models
{
    /// <summary>
    /// Represents the base type for entities that track creation and modification timestamps.
    /// </summary>
    public abstract class Root
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}