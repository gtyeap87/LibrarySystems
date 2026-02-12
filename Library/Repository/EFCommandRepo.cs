using EFCore.BulkExtensions;
using Library.Data;
using Library.Factory;

namespace Library.Repository
{
    public class EfCommandRepo<T>(LibraryContext context, ILogger<EfCommandRepo<T>> logger) : ICommandRepo<T> where T : class
    {
        private readonly LibraryContext _context = context;
        private readonly ILogger<EfCommandRepo<T>> _logger = logger;

        public async Task<Guid> AddAsync(T entity)
        {
            _logger.LogDebug("Adding a new {EntityName} to the library", typeof(T).Name);
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
                    if (_logger.IsEnabled(LogLevel.Debug))
                        _logger.LogDebug("Successfully added {EntityName} with ID {Id}", typeof(T).Name, id);
                    return id;
                }
            }

            return Guid.Empty; // fallback if entity has no Id
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Adding a new {EntityName} to the library", typeof(T).Name);

            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                for (int i = 0; i < entities.Count(); i++)
                {
                    var idValue = idProperty.GetValue(entities.ElementAt(i));
                    if (idValue is Guid id && _logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Successfully added {EntityName} with ID {Id}", typeof(T).Name, id);
                    }
                }
            }
        }

        public async Task BulkInsertAsync<K>(IEnumerable<K> entities) where K : class, IRoot
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Bulk inserting {Count} {EntityName} records", entities.Count(), typeof(T).Name);

            var list = entities.ToList();
            BulkRootFactory.Initialize(list);

            await _context.BulkInsertAsync(list);

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Successfully bulk inserted {Count} {EntityName} records", entities.Count(), typeof(T).Name);
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

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Successfully updated {EntityName} with ID {Id}", typeof(T).Name, id);

            return existingEntity;
        }

        public async Task BulkUpdateAsync<K>(IEnumerable<K> entities, BulkConfig bulkConfig) where K : class, IRoot
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Bulk updating {Count} {EntityName} records", entities.Count(), typeof(T).Name);

            var list = entities.ToList();
            BulkRootFactory.Initialize(list, BulkRootFactory.TranscationType.Update);
            await _context.BulkUpdateAsync(list, bulkConfig);

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Successfully bulk updated {Count} {EntityName} records", entities.Count(), typeof(T).Name);
        }

        public async Task DeleteAsync(T entity)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Deleting {EntityName} from the library", typeof(T).Name);

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Successfully deleted {EntityName}", typeof(T).Name);
        }
    }
}