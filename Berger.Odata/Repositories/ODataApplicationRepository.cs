using Berger.Data.Common;
using Berger.Data.MsfaEntity;

namespace Berger.Odata.Repositories
{
    public class ODataApplicationRepository<TEntity> : ODataRepository<TEntity>, IODataApplicationRepository<TEntity>  where TEntity : class
    {
        public ODataApplicationRepository(ApplicationDbContext context, IUnitOfWork uow) : base(context, uow)
        {
        }
    }
}