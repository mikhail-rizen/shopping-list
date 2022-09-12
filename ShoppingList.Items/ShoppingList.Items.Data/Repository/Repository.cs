using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingList.Items.Data.Database;

namespace ShoppingList.Items.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ItemsContext itemsContext;
        private readonly ILogger<Repository<TEntity>> logger;

        public Repository(ItemsContext itemsContext, ILogger<Repository<TEntity>> logger)
        {
            this.itemsContext = itemsContext;
            this.logger = logger;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            /*
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"{nameof(AddAsync)}: entity must not be null");
            }
            */
            try
            {
                logger.LogInformation("Adding entity {entityType}", entity.GetType().FullName);
                await itemsContext.AddAsync(entity);
                await itemsContext.SaveChangesAsync();
                return entity;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Exception while saving entity");
                throw new Exception($"Exception while saving entity: {e.Message}");
            }
            
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                logger.LogInformation("Retrieving entities {entityType}", typeof(TEntity).FullName);
                return await itemsContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception while retrieving entities");
                throw new Exception($"Exception while retrieving entities: {e.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                logger.LogInformation("Updating entity {entityType}", entity.GetType().FullName);
                itemsContext.Update(entity);
                await itemsContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception while updating entity");
                throw new Exception($"Exception while updating entity: {e.Message}");
            }
        }
    }
}
