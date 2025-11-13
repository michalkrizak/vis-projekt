namespace api.DAL.Interfaces
{
    /// <summary>
    /// Generic DAO interface defining standard CRUD operations.
    /// Based on cv4 TypeScript DAO pattern.
    /// </summary>
    public interface IDao<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
