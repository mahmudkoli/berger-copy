namespace Berger.Worker.Repositories
{
    public interface IApplicationRepository<TEntity> :IRepository<TEntity> where TEntity : class
    {

    }
}