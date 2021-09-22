using Berger.Data.Common;
using Berger.Data.MsfaEntity;

namespace Berger.Worker.Repositories
{
    public class ApplicationRepository<TEntity> : Repository<TEntity>, IApplicationRepository<TEntity>  where TEntity : class
    {
        public ApplicationRepository(ApplicationDbContext context, IUnitOfWork uow) : base(context, uow)
        {
        }
    }
}