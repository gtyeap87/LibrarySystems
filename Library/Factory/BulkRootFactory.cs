namespace Library.Factory
{
    public static class BulkRootFactory
    {
        public static void Initialize<T>(IList<T> entities)
            where T : IRoot
        {
            var now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();

                if (entity.CreatedAt == default)
                    entity.CreatedAt = now;
            }
        }
    }
}