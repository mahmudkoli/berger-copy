using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Users;
using Microsoft.EntityFrameworkCore.Query;
//using X.PagedList;

namespace BergerMsfaApi.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        #region CONFIG

        bool ShareContext { get; set; }

        #endregion

        #region LINQ
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity FindInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        bool IsExist(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        //string CreateId(Expression<Func<TEntity, string>> predicate, string prefix, Expression<Func<TEntity, bool>> where = null,
            //int returnLength = 9, char fillValue = '0');

        #endregion

        #region SQL

        //Task<IEnumerable<T>> ExecuteQueryAsyc<T>(string sqlQuery, params object[] parameters) where T : class;
        Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters);

        IEnumerable<dynamic> DynamicListFromSql(string Sql, Dictionary<string, object> Params, bool isStoredProcedure = false);

        (IList<T> Items, int Total, int TotalFilter) GetDataBySP<T>(string sql, IList<(string Key, object Value, bool IsOut)> parameters);

        #endregion

        #region LINQ ASYNC

        Task<IEnumerable<TEntity>> GetAllAsync();
        //Task<IPagedList<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        //Task<IPagedList<TEntity>> FindAllPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindIncludeAsync
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> CreateAsync(TEntity entity);
        Task<List<TEntity>> CreateListAsync(List<TEntity> items);
        Task<List<TEntity>> UpdateListAsync(List<TEntity> items);
        Task<int> UpdateListiAsync(List<TEntity> items);
        Task<int> DeleteListAsync(List<TEntity> item);
        Task<TEntity> UpdateAsync(TEntity item);
        Task<TEntity> CreateOrUpdateAsync(TEntity item);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        // Task<int> UpdateParentWithChildCollectionAsync<T>(T entity, params Expression<Func<T, object>>[] navigations) where T : BaseEntity;
        // Task<int> UpdateParentWithChildCollectionAsync(TEntity entity, params Expression<Func<TEntity, object>>[] navigations);
        Task<int> CountAsync();
        Task<long> LongCountAsync();
        Task<int> CountFuncAsync(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountFuncAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<string> MaxAsync(Expression<Func<TEntity, string>> predicate);
        Task<string> MaxFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        Task<string> MinAsync(Expression<Func<TEntity, string>> predicate);
        Task<string> MinFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IList<TResult>> GetAllIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true);

        Task<(IList<TResult> Items, int Total, int TotalFilter)> GetAllIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                           Expression<Func<TEntity, bool>> predicate = null,
                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                           int pageIndex = 1, int pageSize = 10,
                           bool disableTracking = true);

        Task<TResult> GetFirstOrDefaultIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true);
        #endregion

        #region SaveChange

        int SaveChanges();

        Task<int> SaveChangesAsync();

        #endregion

        IQueryable<TEntity> GetAllIncludeStrFormat(Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                            string includeProperties = null,
                                                            int? skip = null,
                                                            int? take = null);
        
        IList<UserInfo> GetNodeWiseUsersByUserId(int userId, bool isOnlyLastNodeUser = false);
    }
}

