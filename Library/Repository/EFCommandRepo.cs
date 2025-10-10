using Library.Data;

namespace Library.Repository
{
    public class EFCommandRepo<T>(LibraryContext context, ILogger<EFCommandRepo<T>> logger) : ICommandRepo<T> where T : class
    {
        private readonly LibraryContext _context = context;
        private readonly ILogger<EFCommandRepo<T>> _logger = logger;

        public async Task<Guid> AddAsync(T entity)
        {
            _logger.LogInformation("Adding a new {EntityName} to the library", typeof(T).Name);
            // Add the entity to the DbSet<T>
            _context.Set<T>().Add(entity);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return Id if entity has Id property, else default Guid
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity);
                if (idValue is Guid id)
                {
                    _logger.LogInformation("Successfully added {EntityName} with ID {Id}", typeof(T).Name, id);
                    return id;
                }
            }

            return Guid.Empty; // fallback if entity has no Id
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _logger.LogInformation("Updating {EntityName} information", typeof(T).Name);
            // Try to find the existing record by Id
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                _logger.LogError("Entity {EntityName} has no Id property", typeof(T).Name);
                throw new InvalidOperationException($"Entity {typeof(T).Name} must have an Id property.");
            }

            var idValue = idProperty.GetValue(entity);
            if (idValue is not Guid id)
            {
                _logger.LogError("Entity {EntityName} has invalid Id value", typeof(T).Name);
                throw new InvalidOperationException($"Entity {typeof(T).Name} must have a valid Guid Id.");
            }

            // Retrieve existing entity from database
            var existingEntity = await _context.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                _logger.LogWarning("{EntityName} with ID {Id} not found for update", typeof(T).Name, id);
                throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} not found.");
            }

            // Update current values
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated {EntityName} with ID {Id}", typeof(T).Name, id);

            return existingEntity;
        }

        public async Task DeleteAsync(T entity)
        {
            _logger.LogInformation("Deleting {EntityName} from the library", typeof(T).Name);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted {EntityName}", typeof(T).Name);
        }
    }
}