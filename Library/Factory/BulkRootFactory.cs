using System.Runtime.InteropServices;

namespace Library.Factory
{
    public static class BulkRootFactory
    {
        public static void Initialize<T>(IList<T> entities, [Optional] TranscationType type)
            where T : IRoot
        {
            var now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                if (type == TranscationType.Create)
                {
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();

                    if (entity.CreatedAt == default)
                        entity.CreatedAt = now;
                }

                if (type == TranscationType.Update)
                {
                    entity.ModifiedAt = now;
                }
            }
        }

        public enum TranscationType
        {
            Create,
            Update,
            Delete
        }
    }
}