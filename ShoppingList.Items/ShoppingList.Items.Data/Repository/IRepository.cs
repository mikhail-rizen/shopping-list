namespace ShoppingList.Items.Data.Repository
{
    public interface IRepository<TEntity> where TEntity: class, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
