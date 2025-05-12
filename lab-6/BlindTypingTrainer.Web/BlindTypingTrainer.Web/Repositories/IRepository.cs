namespace BlindTypingTrainer.Web.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class
    {

    }
}
