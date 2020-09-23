using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Organizations;
using Berger.Data.MsfaEntity.WorkFlows;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.WorkFlows;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.WorkFlows.Interfaces;
using Microsoft.Data.SqlClient;

namespace BergerMsfaApi.Services.WorkFlows.Implementation
{
    public class WorkFlowLogHistoryService : IWorkFlowLogHistoryService
    {
        private readonly IRepository<WorkflowLogHistory> _workFlowLogHistory;
        private readonly IRepository<WorkflowLog> _workFlowLog;
        private readonly IRepository<WorkFlow> _workflow;
        private readonly IRepository<WorkFlowConfiguration> _workflowConfig;
        private readonly IRepository<OrganizationUserRole> _orgUserRole;

        public WorkFlowLogHistoryService(IRepository<WorkflowLogHistory> WorkflowLogHistory, 
            IRepository<WorkflowLog> WorkflowLog,
            IRepository<WorkFlow> Workflow,
            IRepository<WorkFlowConfiguration> WorkflowConfig,
            IRepository<OrganizationUserRole> orgUserRole
            )
        {
            _workFlowLogHistory = WorkflowLogHistory;
            _workFlowLog = WorkflowLog;
            _workflow = Workflow;
            _workflowConfig = WorkflowConfig;
            this._orgUserRole = orgUserRole;
        }

        public async Task<WorkFlowLogHistoryModel> CreateAsync(WorkFlowLogHistoryModel model)
        {
            var appUserId = AppIdentity.AppUser.UserId;
            //appUserId = 55;

            var wfLogHistory = model.ToMap<WorkFlowLogHistoryModel, WorkflowLogHistory>();
            wfLogHistory.UserId = appUserId;
            var result = await _workFlowLogHistory.CreateAsync(wfLogHistory);

            #region Workflow Log updated
            var workflowLog = _workFlowLog.Find(x => x.Id == wfLogHistory.WorkflowLogId);

            if(workflowLog != null)
            {
                #region Existing Same WorkflowLog updated
                var workflowLogList = _workFlowLog.FindAll(x => x.TableName == workflowLog.TableName && x.RowId == workflowLog.RowId && 
                    x.MasterWorkFlowId == workflowLog.MasterWorkFlowId && x.OrgRoleId == workflowLog.OrgRoleId).ToList();

                foreach (var item in workflowLogList)
                {
                    item.WorkflowStatus = (int)WorkflowStatus.Completed;
                }
                await _workFlowLog.UpdateListAsync(workflowLogList);
                #endregion

                #region New WorkflowLog added
                //var workflow = _workflow.Find(x => x.Id == workflowLog.MasterWorkFlowId);
                var workflowLogConfigList = _workflowConfig.FindAll(x => x.MasterWorkFlowId == workflowLog.MasterWorkFlowId).OrderBy(x => x.sequence).ToList();
                var currentWorkflowLogConfig = workflowLogConfigList.FirstOrDefault(x => x.OrgRoleId == workflowLog.OrgRoleId);
                var nextWorkflowConfig = workflowLogConfigList.FirstOrDefault(x => x.sequence > currentWorkflowLogConfig.sequence);
                
                if(nextWorkflowConfig != null)
                {
                    var newWorkflowLogList = new List<WorkflowLog>();
                    var userIds = _orgUserRole.FindAll(x => x.OrgRoleId == nextWorkflowConfig.OrgRoleId).OrderBy(x => x.UserSequence)
                        .Select(x => x.UserId).ToList();

                    foreach (var userId in userIds)
                    {
                        var newWorkflowLog = new WorkflowLog
                        {
                            RowId = workflowLog.RowId,
                            MasterWorkFlowId = workflowLog.MasterWorkFlowId,
                            WorkflowStatus = (int)WorkflowStatus.Pending,
                            WorkFlowFor = userId,
                            TableName = workflowLog.TableName,
                            OrgRoleId = nextWorkflowConfig.OrgRoleId

                        };
                        newWorkflowLogList.Add(newWorkflowLog);
                    }

                    if(newWorkflowLogList.Any())
                        await _workFlowLog.CreateListAsync(newWorkflowLogList);
                }
                #endregion

                #region Entity Status Updated
                var status = model.WorkflowStatus == WorkflowStatus.Approved ?
                    (nextWorkflowConfig == null ? WorkflowStatus.Approved : WorkflowStatus.ApprovalInProgress) : WorkflowStatus.Rejected; 
                if(status == WorkflowStatus.Approved && workflowLog.TableName == "DailyCMActivities")
                {
                    await _workFlowLog.ExecuteSqlCommandAsync($"UPDATE {workflowLog.TableName} SET STATUS = @Status, WFSTATUS = @WFStatus WHERE Id = @id",
                                    new SqlParameter("@WFStatus", status), new SqlParameter("@Status", Status.Pending), new SqlParameter("@Id", workflowLog.RowId));
                }
                else
                {
                    await _workFlowLog.ExecuteSqlCommandAsync($"UPDATE {workflowLog.TableName} SET WFSTATUS = @Status WHERE Id = @id",
                                    new SqlParameter ("@Status", status), new SqlParameter("@Id", workflowLog.RowId));
                }
                #endregion
            }
            #endregion

            return result.ToMap<WorkflowLogHistory, WorkFlowLogHistoryModel>();
        }

        public async Task<IEnumerable<WorkFlowLogHistoryModel>> GetWorkFlowLogHistoryForCurrentUserAsync(int pageNumber, int pageSize)
        {
            var userId = AppIdentity.AppUser.UserId;
            //userId = 55;
            var result = _workFlowLogHistory.GetAllIncludeStrFormat(m => m.UserId == userId,
                or => or.OrderByDescending(o => o.CreatedTime), "User").ToList();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkflowLogHistory, WorkFlowLogHistoryModel>()
                    .ForMember(src => src.UserName, opt => opt.MapFrom(dest => dest.User.Name));
            }).CreateMapper();

            var data = mapper.Map<List<WorkFlowLogHistoryModel>>(result);
            return data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _workFlowLogHistory.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<WorkFlowLogHistoryModel> GetWorkFlowLogHistoryAsync(int id)
        {
            var result = await _workFlowLogHistory.FindAsync(s => s.Id == id);
            return result.ToMap<WorkflowLogHistory, WorkFlowLogHistoryModel>();
        }

        public async Task<IEnumerable<WorkFlowLogHistoryModel>> GetWorkFlowLogHistoriesAsync()
        {
            var result = await _workFlowLogHistory.GetAllAsync();
            return result.ToMap<WorkflowLogHistory, WorkFlowLogHistoryModel>();
        }


        public async Task<WorkFlowLogHistoryModel> SaveAsync(WorkFlowLogHistoryModel model)
        {
            var example = model.ToMap<WorkFlowLogHistoryModel, WorkflowLogHistory>();
            var result = await _workFlowLogHistory.CreateOrUpdateAsync(example);
            return result.ToMap<WorkflowLogHistory, WorkFlowLogHistoryModel>();
        }

        public async Task<WorkFlowLogHistoryModel> UpdateAsync(WorkFlowLogHistoryModel model)
        {
            var example = model.ToMap<WorkFlowLogHistoryModel, WorkflowLogHistory>();
            var result = await _workFlowLogHistory.UpdateAsync(example);
            return result.ToMap<WorkflowLogHistory, WorkFlowLogHistoryModel>();
        }


    }
}

