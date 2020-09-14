using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BergerMsfaApi.Domain.Organizations;
using BergerMsfaApi.Domain.WorkFlows;
using BergerMsfaApi.Enumerations;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.WorkFlows;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.WorkFlows.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.WorkFlows.Implementation
{
    public class WorkFlowLogService : IWorkFlowLogService
    {
        private readonly IRepository<WorkflowLog> _workFlowLog;
        private readonly IRepository<WorkflowLogHistory> _workflowHistory;
        private readonly IRepository<WorkFlow> _workFlow;
        private readonly IRepository<WorkFlowType> _workFlowType;
        private readonly IRepository<WorkFlowConfiguration> _workflowConfig;
        private readonly IRepository<OrganizationUserRole> _orgUserRole;

        public WorkFlowLogService(IRepository<WorkflowLog> workflowLog, 
            IRepository<WorkflowLogHistory> workflowLogHistory,
            IRepository<WorkFlow> workFlow,
            IRepository<WorkFlowType> workFlowType,
            IRepository<WorkFlowConfiguration> WorkflowConfig,
            IRepository<OrganizationUserRole> orgUserRole)
        {
            _workFlowLog = workflowLog;
            _workflowHistory = workflowLogHistory;
            _workFlow = workFlow;
            _workFlowType = workFlowType;
            _workflowConfig = WorkflowConfig;
            _orgUserRole = orgUserRole;
        }

        public async Task<WorkFlowLogModel> CreateAsync(WorkFlowLogModel model)
        {
            var example = model.ToMap<WorkFlowLogModel, WorkflowLog>();
            var result = await _workFlowLog.CreateAsync(example);
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _workFlowLog.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsWorkFlowLogExistAsync(int masterWorkFlowId)
        {
            var result = masterWorkFlowId <= 0;
            if (await _workFlowLog.IsExistAsync(s => s.MasterWorkFlowId == masterWorkFlowId))
            {
                return true;
            }
            return result;
        }
        public async Task<WorkFlowLogModel> GetWorkFlowLogAsync(int id)
        {
            var result = await _workFlowLog.FindAsync(s => s.Id == id);
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();
        }

        public async Task<IEnumerable<WorkFlowLogModel>> GetWorkFlowLogsByWorkflowIdAsync(int id)
        {
            var result = await _workFlowLog.FindAllAsync(s => s.MasterWorkFlowId == id);
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();
        }

        public async Task<int> GetUnseenHistoryCountByUserId(int id)
        {
            var result = await _workFlowLog.ExecuteQueryAsyc<WorkFlowLogModel>("SELECT * FROM WorkFlowLogs");
            var workFlowLogModels = result.ToList();
            return workFlowLogModels[0]?.PendingWorkflowCount ?? 0;
        }

        public async Task<IEnumerable<WorkFlowLogModel>> GetWorkFlowLogForCurrentUserAsync(int? status, int pageNumber, int pageSize)
        {
            var appUserId = AppIdentity.AppUser.UserId;
            //appUserId = 55;

            var result = _workFlowLog.GetAllIncludeStrFormat(m => m.WorkFlowFor == appUserId && (!status.HasValue || m.WorkflowStatus == status), 
                or => or.OrderByDescending(x => x.CreatedTime), "WorkFlow").ToList();
            var data = result.ToMap<WorkflowLog, WorkFlowLogModel>();

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var workflowLog = result[i];

                #region Get message and data
                var propertyMessage = "";

                var workflowTypeId = workflowLog.WorkFlow.WorkflowType;
                var workflowType = _workFlowType.Find(x => x.Id == workflowTypeId);
                var dynamicObject = _workFlowLog.DynamicListFromSql(workflowType.ViewName,
                                        new Dictionary<string, object> { { "Id", workflowLog.RowId } }, true).FirstOrDefault();

                var expandoDict = dynamicObject as IDictionary<string, object>;
                if(expandoDict != null)
                {
                    if (expandoDict.ContainsKey("Name"))
                        propertyMessage = expandoDict["Name"].ToString();
                    else if (expandoDict.Keys.Any(k => k.StartsWith("Name") || k.EndsWith("Name")))
                    {
                        var key = expandoDict.Keys.FirstOrDefault(k => k.StartsWith("Name") || k.EndsWith("Name"));
                        propertyMessage = expandoDict[key].ToString();
                    }
                }

                //var newDynamicObject = new Dictionary<string, object>();
                //foreach (var dict in expandoDict)
                //{
                //    if (dict.Key.StartsWith("Created") || dict.Key.StartsWith("Modified") ||
                //        dict.Key == "Id") continue;

                //    newDynamicObject.Add(dict.Key, dict.Value);
                //}
                #endregion

                //var wf = await _workFlow.FindAsync(x => x.Id == item.MasterWorkFlowId);
                var workflow = workflowLog.WorkFlow;
                var wfType = await _workFlowType.FindAsync(wft => wft.Id == workflow.WorkflowType);
                item.WorkflowMessage = string.Format(wfType.WorkflowMessage, "\""+propertyMessage+"\"");

                //item.PendingWorkflowCount = data.Count;
                //item.Data = dynamicObject;

                item.PendingWorkflowCount = data.Count(x => (int)x.WorkflowStatus == (int)Status.Pending);
                item.Data = dynamicObject;


                #region Get workflow history
                var workflowLogConfigList = await _workflowConfig.FindAllAsync(x => x.MasterWorkFlowId == workflowLog.MasterWorkFlowId);
                   workflowLogConfigList =  workflowLogConfigList.OrderBy(x => x.sequence).ToList();
                var currentWorkflowLogConfig = workflowLogConfigList.FirstOrDefault(x => x.OrgRoleId == workflowLog.OrgRoleId);
                var preWorkflowConfig = workflowLogConfigList.Where(x => x.sequence <= currentWorkflowLogConfig.sequence).AsEnumerable();

                if(preWorkflowConfig.Any())
                {

                    //var userIds = new List<int>();
                    //foreach (var org in preWorkflowConfig)
                    //{
                    //    var usids = _orgUserRole.FindAll(x => org.OrgRoleId == x.OrgRoleId)
                    //        .OrderBy(x => x.UserSequence).Select(x => x.UserId).ToList();
                    //    if (usids.Any()) userIds.AddRange(usids);
                    //}

                    //var workflowLogHistory = new List<WorkflowLogHistory>();

                    //foreach (var usId in userIds)
                    //{
                    //    var wfLogHis = _workflowHistory.GetAllIncludeStrFormat(x => x.WorkflowLog.WorkFlowFor == usId &&
                    //        x.WorkflowLog.TableName == workflowLog.TableName && x.WorkflowLog.RowId == workflowLog.RowId, null, "User").ToList();
                    //    if (wfLogHis.Any()) workflowLogHistory.AddRange(wfLogHis);
                    //}

                    //preWorkflowConfig.Any(w => w.OrgRoleId == x.OrgRoleId)
                    var userIdList = await _orgUserRole.FindAllAsync(x =>  preWorkflowConfig.Select(a=> a.OrgRoleId).Contains(x.OrgRoleId));
                    var userIds =  userIdList.OrderBy(x => x.UserSequence).Select(x => x.UserId).ToList();

                    var workflowLogHistory =  _workflowHistory.FindAllInclude(x => userIds.Any(u => u == x.WorkflowLog.WorkFlowFor) && 
                        x.WorkflowLog.TableName == workflowLog.TableName && x.WorkflowLog.RowId == workflowLog.RowId, x => x.User).OrderBy(x => x.CreatedBy).ToList();


                    var mapper = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<WorkflowLogHistory, WorkFlowLogHistoryModel>()
                            .ForMember(src => src.UserName, opt => opt.MapFrom(dest => dest.User.Name));
                    }).CreateMapper();

                    item.LogHistories = mapper.Map<List<WorkFlowLogHistoryModel>>(workflowLogHistory.OrderByDescending(x => x.CreatedTime));
                }
                #endregion
            }

            return data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        
        public async Task<IEnumerable<WorkFlowLogModel>> GetWorkFlowLogsAsync()
        {
            var result = await _workFlowLog.GetAllAsync();
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();
        }

        public async Task<IPagedList<WorkFlowLogModel>> GetPagedWorkFlowLogsAsync(int pageNumber, int pageSize)
        {
            var result = await _workFlowLog.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();

        }
        public async Task<IPagedList<WorkFlowLogModel>> GetPagedQueryWorkFlowLogsAsync(int pageNumber, int pageSize)
        {
            var result = await _workFlowLog.ExecuteQueryAsyc<WorkFlowLogModel>("SELECT wfc.*,org.[Name] AS [OrgRoleName],wf.[Name] AS [MasterWorkFlowName] FROM [dbo].[WorkFlowLogs] AS wfc INNER JOIN[dbo].[WorkFlows] AS wf ON wfc.MasterWorkFlowId = wf.Id INNER JOIN[dbo].[OrganizationRoles] AS org ON wfc.OrgRoleId = org.Id");
            return await result.AsQueryable().ToPagedListAsync(pageNumber, pageSize);

        }
        //.AsQueryable().ToPagedListAsync(pageNumber, pageSize);
        public async Task<IEnumerable<WorkFlowLogModel>> GetQueryWorkFlowLogsAsync()
        {
            var result = await _workFlowLog.ExecuteQueryAsyc<WorkFlowLogModel>("SELECT * FROM WorkFlowLogs");
            return result;
        }

        public async Task<WorkFlowLogModel> SaveAsync(WorkFlowLogModel model)
        {
            var example = model.ToMap<WorkFlowLogModel, WorkflowLog>();
            var result = await _workFlowLog.CreateOrUpdateAsync(example);
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();
        }

        public async Task<WorkFlowLogModel> UpdateAsync(WorkFlowLogModel model)
        {
            var example = model.ToMap<WorkFlowLogModel, WorkflowLog>();
            var result = await _workFlowLog.UpdateAsync(example);
            return result.ToMap<WorkflowLog, WorkFlowLogModel>();
        }


    }
}

