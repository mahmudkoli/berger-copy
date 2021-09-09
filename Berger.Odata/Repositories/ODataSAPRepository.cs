using Berger.Data.Common;
using Berger.Data.MsfaEntity;

namespace Berger.Odata.Repositories
{
    public class ODataSAPRepository<TEntity> : ODataRepository<TEntity>, IODataSAPRepository<TEntity>  where TEntity : class
    {
        public ODataSAPRepository(SAPDbContext context, IUnitOfWork uow) : base(context, uow)
        {
        }
    }
}