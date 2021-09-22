using Berger.Data.Common;
using Berger.Data.MsfaEntity;

namespace Berger.Worker.Repositories
{
    public class SAPRepository<TEntity> : Repository<TEntity>, ISAPRepository<TEntity>  where TEntity : class
    {
        public SAPRepository(SAPDbContext context, IUnitOfWork uow) : base(context, uow)
        {
        }
    }
}