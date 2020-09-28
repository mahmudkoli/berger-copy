using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Berger.Common.Enumerations;
using Berger.Data;
using Berger.Data.Attributes;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.Organizations;
using Berger.Data.MsfaEntity.Users;
using Berger.Data.MsfaEntity.WorkFlows;
using BergerMsfaApi.Core;
using BergerMsfaApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace BergerMsfaApi.Repositories
{
    
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region CONFIG

        private DbContext _context;
        private bool _shareContext;
        private bool _disposed;
        private readonly IUnitOfWork _uow;
        public bool ShareContext
        {
            get { return _shareContext; }
            set { _shareContext = value; }
        }



        public Repository(DbContext context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
            //Context.Database.CommandTimeout = 100000;
            // DbInterception.Add(new AzRInterceptor());
        }

        protected DbSet<TEntity> DbSet
        {
            get
            {
                return _context.Set<TEntity>();
            }
            set
            {
                value = _context.Set<TEntity>();
            }
        }


        #endregion

        #region Disposed

        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (ShareContext || _context == null) return;
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
            }
            _disposed = true;
        }

        #endregion

        #region LINQ

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking().AsQueryable();

        }

        public IQueryable<TEntity> FindAllInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable().Where(predicate), (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> GetAllInclude
            (params Expression<Func<TEntity, object>>[] includeProperties)
        {

            return includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty));
        }
        public TEntity FindInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty)).FirstOrDefault(predicate);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }


        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking().AsQueryable();
        }


        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count > 0;
        }

        public string CreateId(Expression<Func<TEntity, string>> predicate, string prefix, Expression<Func<TEntity, bool>> where = null,
            int returnLength = 9, char fillValue = '0')
        {
            return where != null ? DbSet.Where(where).AsNoTracking().Max(predicate).MakeId(prefix, returnLength, fillValue)
                : DbSet.AsNoTracking().Max(predicate).MakeId(prefix, returnLength, fillValue);
        }

        #endregion

        #region SQL
        public async Task<IEnumerable<T>> ExecuteQueryAsyc<T>(string sqlQuery, params object[] parameters) where T : class
        {
            return await _context.SqlQueryAsync<T>(sqlQuery, parameters);
        }
        public async Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sqlCommand, parameters);
        }

        public IEnumerable<dynamic> DynamicListFromSql(string Sql, Dictionary<string, object> Params, bool isStoredProcedure = false)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if(isStoredProcedure)
                    cmd.CommandType = CommandType.StoredProcedure; 
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

                foreach (KeyValuePair<string, object> p in Params)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = p.Key;
                    dbParameter.Value = p.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                        {
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
                        }
                        yield return row;
                    }
                }
            }
        }

        public (IList<T> Items, int Total, int TotalFilter) GetDataBySP<T>(string sql, IList<(string Key, object Value, bool IsOut)> parameters)
        {
            var items = new List<T>();
            int? totalCount = 0;
            int? filteredCount = 0;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                if (command.Connection.State != ConnectionState.Open) { command.Connection.Open(); }

                foreach (var param in parameters)
                {
                    DbParameter dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    if (!param.IsOut)
                    {
                        dbParameter.Value = param.Value;
                    }
                    else
                    {
                        dbParameter.Direction = ParameterDirection.Output;
                        dbParameter.DbType = DbType.Int32;
                    }
                    command.Parameters.Add(dbParameter);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var itemType = typeof(T);
                        var constructor = itemType.GetConstructor(new Type[] { });
                        var instance = constructor.Invoke(new object[] { });
                        var properties = itemType.GetProperties();

                        foreach (var property in properties)
                        {
                            if(!reader.IsDBNull(property.Name))
                                property.SetValue(instance, reader[property.Name]);
                        }

                        items.Add((T)instance);
                    }
                }

                totalCount = (int?)command.Parameters["TotalCount"].Value;
                filteredCount = (int?)command.Parameters["FilteredCount"].Value;
            }

            return (items, totalCount ?? 0, filteredCount ?? 0);
        }


        #endregion

        #region LINQ ASYNC

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return await DbSet.AsNoTracking().AsQueryable().ToPagedListAsync(pageNumber, pageSize);

        }
        public async Task<IPagedList<TEntity>> FindAllPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize)
        {
            return await DbSet.Where(predicate).AsNoTracking().AsQueryable().ToPagedListAsync(pageNumber, pageSize);

        }
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FindIncludeAsync
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty)).FirstOrDefaultAsync(predicate);
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> CreateAsync(TEntity item)
        {
            string tableName = GetTableName();
            var IsWorkFlowAvailalbe = await IsWfEnabledForEntityGetWf(tableName);
            if (IsWorkFlowAvailalbe != null)
            {
                item.GetType().GetProperty("WorkflowId")?.SetValue(item, IsWorkFlowAvailalbe.Id);
                item.GetType().GetProperty("WFStatus")?.SetValue(item, WorkflowStatus.Pending);
                DbSet.Add(item);
                await SaveChangesAsync();
                //Workflow Log Entry
                var insertedId = item.GetType().GetProperty("Id").GetValue(item);
                var workFlowId = IsWorkFlowAvailalbe.Id;

                int rowId = (int?) insertedId ?? 0;
                await InsertIntoWorkFlowLogTable(rowId, workFlowId, tableName);
            }
            else
            {
                DbSet.Add(item);
            }


            await SaveChangesAsync();
            return item;
        }
        public async Task<List<TEntity>> CreateListAsync(List<TEntity> items)
        {
            DbSet.AddRange(items);
            await SaveChangesAsync();
            return items;
        }
        public async Task<List<TEntity>> UpdateListAsync(List<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            try
            {
                foreach (var item in items)
                {
                    var entry = _context.Entry(item);
                    DbSet.Attach(item);
                    entry.State = EntityState.Modified;
                }
                var result = await SaveChangesAsync();
                return result > 0 ? items : null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> DeleteListAsync(List<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            try
            {

                foreach (var record in items)
                {
                    DbSet.Attach(record);

                }
                DbSet.RemoveRange(items);
                var result = await SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
           
            DbSet.Attach(item);
            var entry = _context.Entry(item);
            entry.State = EntityState.Modified;
            await SaveChangesAsync();
         //  _uow.Commit();
           return item;
        }
        public async Task<TEntity> CreateOrUpdateAsync(TEntity item)
        {            
            var pi = item.GetType().GetProperty("Id");
            var keyFieldId = pi != null ? pi.GetValue(item, null) : -1;
            keyFieldId = keyFieldId == (object)0 ? -1 : keyFieldId;

            var record = await DbSet.FindAsync(keyFieldId);
            if (record == null)
            {
                string tableName = GetTableName();
                var IsWorkFlowAvailalbe = await IsWfEnabledForEntityGetWf(tableName);
                if (IsWorkFlowAvailalbe != null)
                {
                    item.GetType().GetProperty("WorkflowId").SetValue(item, IsWorkFlowAvailalbe.Id);
                    item.GetType().GetProperty("WFStatus").SetValue(item, WorkflowStatus.Pending);
                    DbSet.Add(item);
                    await SaveChangesAsync();
                    //Workflow Log Entry
                    var insertedId = item.GetType().GetProperty("Id").GetValue(item);
                    var workFlowId = IsWorkFlowAvailalbe.Id;

                    int rowId = insertedId != null ? (int)insertedId : 0;
                    await InsertIntoWorkFlowLogTable(rowId, workFlowId, tableName);
                }
                else
                {
                    DbSet.Add(item);
                }


            }
            else
            {
                _context.Entry(record).CurrentValues.SetValues(item);
            }

            var result = !ShareContext ? await SaveChangesAsync() : 0;
            return result > 0 ? item : null;
        }
        #region Workflow
        private async Task<int> InsertIntoWorkFlowLogTable(int rowId, int workflowId, string tableName)
        {
            //---Agenda 1: First Collect All User and insert each user into workflow for

            int result = 0;
            //Context

            var wfConfigContext = _context.Set<WorkFlowConfiguration>();
            var wfOrgUserRoleContext = _context.Set<OrganizationUserRole>();
            var wfOrgRoleContext = _context.Set<OrganizationRole>();
            var userTerrContext = _context.Set<UserTerritoryMapping>();


            //get initial role from wfConfig

            var wfConfigdata = await wfConfigContext.Include(i => i.OrganizationRole).OrderBy(a => a.sequence).FirstOrDefaultAsync(o => o.MasterWorkFlowId == workflowId);

            if (wfConfigdata != null)
            {
                //get orgRoleDetail With Current Position
                var orgRole = wfConfigdata.OrganizationRole;
                //get all user of that organization role
                var orgRoleUsers = await wfOrgUserRoleContext.OrderBy(s => s.UserSequence)
                    .Where(a => a.OrgRoleId == wfConfigdata.OrgRoleId).Select(a => a.UserId).ToListAsync();


                //---------------------------------
                //Current User details, becuase current user parent will receive wf. Which parent? it will determine by his/her org role designation

                var userTerrNodeIds = await userTerrContext.Where(a => a.UserInfoId == AppIdentity.AppUser.UserId).Select(a => a.NodeId).ToListAsync();


                List<int> currentUserParentId = await GetCurrentUserParentByNode(userTerrContext, orgRole, userTerrNodeIds);
                if (currentUserParentId.Count > 0)
                 orgRoleUsers.AddRange(currentUserParentId);


                //var userData = await wfOrgUserRoleContext.OrderBy(s=> s.UserSequence).Where(a => a.OrgRoleId == wfConfigdata.OrgRoleId).ToListAsync();

                if (orgRoleUsers.Count > 0)
                {
                    foreach (var item in orgRoleUsers)
                    {
                        var wfLog = new WorkflowLog
                        {
                            RowId = rowId,
                            MasterWorkFlowId = workflowId,
                            WorkflowStatus = (int)WorkflowStatus.Pending,
                            WorkFlowFor = item,
                            TableName = tableName,
                            OrgRoleId = orgRole.Id

                        };
                        var wfLogContext = _context.Set<WorkflowLog>();
                        wfLogContext.Add(wfLog);
                    }
                    result = await SaveChangesAsync();

                }
                else
                {
                    result = -1;
                }
            }

            return result;
        }

        private async Task<List<int>> GetCurrentUserParentByNode(DbSet<UserTerritoryMapping> userTerrContext, OrganizationRole orgRole, List<int> nodeIds)
        {
            List<int> userId = new List<int>();

            foreach (var item in nodeIds)
            {
                Dictionary<string, int> allParentOfCurrentUserNode = await GetUserIdByParentNode(item, new Dictionary<string, int>());

                int parentNodeOfCurrentUserBasedOnOrgRole = 0;

                if (orgRole.DesignationId == (int)OrgRoleDesignation.Area)
                {
                    parentNodeOfCurrentUserBasedOnOrgRole = allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("A")).Value;
                }
                else if (orgRole.DesignationId == (int)OrgRoleDesignation.Region)
                {
                    parentNodeOfCurrentUserBasedOnOrgRole = allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("R")).Value;
                }
                else if (orgRole.DesignationId == (int)OrgRoleDesignation.National)
                {
                    parentNodeOfCurrentUserBasedOnOrgRole = allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("N")).Value;
                }
                else if (orgRole.DesignationId == (int)OrgRoleDesignation.Territory)
                {
                    parentNodeOfCurrentUserBasedOnOrgRole = allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("T")).Value;
                }
                else
                {

                }
                if (parentNodeOfCurrentUserBasedOnOrgRole > 0)
                {
                    var getUserFromUserTerrUsingNode = await userTerrContext.FirstOrDefaultAsync(a => a.NodeId == parentNodeOfCurrentUserBasedOnOrgRole);
                    if (getUserFromUserTerrUsingNode != null)
                        userId.Add(getUserFromUserTerrUsingNode.UserInfoId);
                }
            }          
            
            return userId;
        }

        public async Task<Dictionary<string, int>> GetUserIdByParentNode(int nodeid, Dictionary<string, int> dict) 
        {
        //    var nodeContext = _context.Set<Node>();
            
            
        //    var checkTerrNode = await nodeContext.FirstOrDefaultAsync(a => a.NodeId == nodeid);
            
        //    if (checkTerrNode != null)
        //    {
        //        dict.Add(checkTerrNode.Code, nodeid);
        //        if (checkTerrNode.ParentId != null)
        //        {                   
        //           await GetUserIdByParentNode(checkTerrNode.ParentId??0, dict);
        //        }
        //    }
        //    else
        //    {

        //    }
                return new Dictionary<string, int>();
        }

        public async Task<WorkFlow> IsWfEnabledForEntityGetWf(string tableName)
        {
            //Checking table Availble in WorkflowType
            var contextforWorkflowType = _context.Set<WorkFlowType>();
            var isWorkflowAvailable = await contextforWorkflowType.FirstOrDefaultAsync(a => a.DbTableName.ToUpper() == tableName.ToUpper() && a.IsWorkflowConfigAvailable == true && a.IsWorkflowDefAvailable == true);
            if (isWorkflowAvailable != null)
            {
                //get workflow data
                var contextForWorkflow = _context.Set<WorkFlow>();
                var data = await contextForWorkflow.FirstOrDefaultAsync(a => a.WorkflowType == isWorkflowAvailable.Id);
                return data;
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> CheckWfEnabledForEntity(string tableName)
        {
            var contextforWorkflowType = _context.Set<WorkFlowType>();
            var isWorkflowAvailable = await contextforWorkflowType.AnyAsync(a => a.DbTableName.ToUpper() == tableName.ToUpper() && a.IsWorkflowConfigAvailable == true && a.IsWorkflowDefAvailable == true);
            return isWorkflowAvailable;
        }
        #endregion
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var records = await DbSet.Where(predicate).ToListAsync();
            if (!records.Any())
            {
                throw new Exception(".NET ObjectNotFoundException"); //new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return await SaveChangesAsync();
        }


        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }
        public async Task<long> LongCountAsync()
        {
            return await DbSet.LongCountAsync();
        }
        public async Task<int> CountFuncAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }
        public async Task<long> LongCountFuncAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.LongCountAsync(predicate);
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<string> MaxAsync(Expression<Func<TEntity, string>> predicate)
        {
            return await DbSet.MaxAsync(predicate);
        }
        public async Task<string> MaxFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().MaxAsync(predicate);
        }
        public async Task<string> MinAsync(Expression<Func<TEntity, string>> predicate)
        {
            return await DbSet.AsNoTracking().MinAsync(predicate);
        }
        public async Task<string> MinFuncAsync(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().MinAsync(predicate);
        }
        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await DbSet.CountAsync(predicate);
            return count > 0;
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public virtual async Task<IList<TResult>> GetAllIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Select(selector).ToListAsync();

            return result;
        }

        public virtual async Task<(IList<TResult> Items, int Total, int TotalFilter)> GetAllIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                           Expression<Func<TEntity, bool>> predicate = null,
                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                           int pageIndex = 1, int pageSize = 10,
                           bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();

            int total = await query.CountAsync();
            int totalFilter = total;

            if (include != null)
                query = include(query);

            if (predicate != null)
            {
                query = query.Where(predicate);
                totalFilter = await query.CountAsync();
            }

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selector).ToListAsync();

            return (result, total, totalFilter);
        }


        public virtual async Task<TResult> GetFirstOrDefaultIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Select(selector).FirstOrDefaultAsync();

            return result;
        }

        #endregion

        #region SaveChange

        public int SaveChanges()
        {
            try
            {
                int resultLog;
                int result;
                var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

                using (var scope = new TransactionScope())
                {
                    bool saveFailed;

                    do
                    {
                        try
                        {
                            resultLog = CreateLog();
                            result = _context.SaveChanges();
                            saveFailed = false;
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;
                            resultLog = 0;
                            result = 0;
                            if (ex.Entries != null && ex.Entries.Any())
                            {
                                ex.Entries.ToList()
                                    .ForEach(entry =>
                                    {
                                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                    });
                            }


                        }
                    } while (saveFailed);

                    scope.Complete();
                }
                return addedEntries.Any() ? resultLog : result;
            }
            //catch (DbEntityValidationException ex)
            //{
            //    var exc = EntityValidationException(ex);
            //    WriteErrorLog(new Exception(exc));
            //    throw new DbEntityValidationException(exc, ex.EntityValidationErrors);

            //}
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {

            try
            {
                int resultLog;
                int result;
                var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    bool saveFailed;
                    do
                    {
                        try
                        {
                            resultLog = CreateLog();
                            result = await _context.SaveChangesAsync();
                            saveFailed = false;
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;
                            result = 0;
                            resultLog = 0;
                            if (ex.Entries != null && ex.Entries.Any())
                            {
                                ex.Entries.ToList()
                                    .ForEach(entry =>
                                    {
                                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                    });
                            }

                        }
                    } while (saveFailed);
                    scope.Complete();
                }
                return addedEntries.Any() ? resultLog : result;
            }
            //catch (DbEntityValidationException ex)
            //{
            //    var exc = EntityValidationException(ex);
            //    WriteErrorLog(new Exception(exc));
            //    throw new DbEntityValidationException(exc, ex.EntityValidationErrors);
            //}
            catch (Exception ex)
            {
                throw;
            }
        }

        private int CreateLog()
        {
            var time = DateTime.Now;
            var userId = AppIdentity.AppUser.UserId;
            var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            if (addedEntries.Count > 0)
            {
                foreach (var entry in addedEntries)
                {
                    if (!(entry.Entity is IAuditableEntity addAudit)) continue;


                    addAudit.CreatedBy = userId;
                    addAudit.CreatedTime = time;
                    addAudit.ModifiedBy = userId;
                    addAudit.ModifiedTime = time;
                }

            }

            var modifiedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();

            if (modifiedEntries.Count > 0)
            {
                foreach (var entry in modifiedEntries)
                {
                    if (entry.Entity is IAuditableEntity modifyAudit)
                    {
                        modifyAudit.ModifiedBy = userId;
                        modifyAudit.ModifiedTime = time;

                    }

                    var properties = typeof(TEntity).GetProperties()
                        .Where(property =>
                            property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                        .Select(p => p.Name);

                    foreach (var property in properties)
                    {
                        entry.Property(property).IsModified = false;
                    }
                }
            }

            var deleteEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            if (deleteEntries.Count > 0)
            {
                foreach (var entry in deleteEntries)
                {
                    if (entry.Entity is IAuditableEntity deleteAudit)
                    {
                        deleteAudit.ModifiedBy = userId;
                        deleteAudit.ModifiedTime = DateTime.Now;

                    }

                    var properties = typeof(TEntity).GetProperties()
                        .Where(property =>
                            property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                        .Select(p => p.Name);
                    foreach (var property in properties)
                    {
                        entry.Property(property).IsModified = false;
                    }
                }


            }

            if (addedEntries.Count <= 0 && modifiedEntries.Count <= 0 && deleteEntries.Count <= 0) return 0;

            return 1;
        }

        //private static string EntityValidationException(DbEntityValidationException ex)
        //{
        //    var outputLines = new List<string>();
        //    foreach (var eve in ex.EntityValidationErrors)
        //    {
        //        outputLines.Add($"{DateTime.Now:MMM dd, yyyy h:mm tt}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:\n");
        //        outputLines.AddRange(eve.ValidationErrors.Select(ve => $"Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n"));
        //    }


        //    // Retrieve the error messages as a list of strings.
        //    var errorMessages = ex.EntityValidationErrors
        //        .SelectMany(x => x.ValidationErrors)
        //        .Select(x => x.ErrorMessage);

        //    // Join the list to a single string.
        //    var fullErrorMessage = string.Join("; ", errorMessages);

        //    // Combine the original exception message with the new one.
        //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //    //GeneralHelper.WriteValue(string.Join("\n", outputLines));
        //    //GeneralHelper.WriteValue(string.Join("\n", exceptionMessage));


        //    // Throw a new DbEntityValidationException with the improved exception message.
        //    return exceptionMessage;
        //}

        #endregion

        public string GetTableName()
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            //var schema = entityType.GetSchema();
            return entityType.GetTableName();
        }

        public IQueryable<TEntity> GetAllIncludeStrFormat
                (Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        string includeProperties = null,
                                                        int? skip = null,
                                                        int? take = null)
        {
            includeProperties ??= string.Empty;
            //IQueryable<TEntity> query = _dbContext.Set<T>();
            IQueryable<TEntity> query = DbSet.AsNoTracking().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public IList<UserInfo> GetNodeWiseUsersByUserId(int userId, bool isOnlyLastNodeUser = false)
        {
            throw new NotImplementedException();
        }
    }
}
