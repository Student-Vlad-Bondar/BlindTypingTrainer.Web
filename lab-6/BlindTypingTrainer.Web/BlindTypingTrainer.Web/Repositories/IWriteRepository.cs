namespace BlindTypingTrainer.Web.Repositories
{
    public interface IWriteRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
