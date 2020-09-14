using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Domain.WorkFlows;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.WorkFlows;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.WorkFlows.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BergerMsfaApi.Services.WorkFlows.Implementation
{
    public class WorkFlowConfigurationService : IWorkFlowConfigurationService
    {
        private readonly IRepository<WorkFlowConfiguration> _workFlowConfiguration;
        private readonly IRepository<WorkFlowType> _workflowtype;
        public WorkFlowConfigurationService(IRepository<WorkFlowConfiguration> workFlowConfiguration, IRepository<WorkFlowType> workflowtype)
        {
            _workFlowConfiguration = workFlowConfiguration;
            _workflowtype = workflowtype;
        }


        public async Task<int> DeleteAsync(int id)
        {
            var result = await _workFlowConfiguration.DeleteAsync(s => s.Id == id);
            return result;

        }


        public async Task<bool> IsOrganizationRoleExistAsync(int masterWorkFlowId, int orgRoleId, int id)
        {
            var result = id > 0
            ? await _workFlowConfiguration.IsExistAsync(s => (s.MasterWorkFlowId == masterWorkFlowId && s.OrgRoleId == orgRoleId) && s.Id != id)
            : await _workFlowConfiguration.IsExistAsync(s => s.MasterWorkFlowId == masterWorkFlowId && s.OrgRoleId == orgRoleId);

            return result;
        }



        public async Task<bool> IsSequenceExistAsync(int masterWorkFlowId, int sequence)
        {
            var result = await _workFlowConfiguration.IsExistAsync(s => s.MasterWorkFlowId == masterWorkFlowId && s.sequence == sequence);

            return result;
        }
        public async Task<WorkFlowConfigurationModel> GetWorkFlowConfigurationAsync(int id)
        {
            var result = await _workFlowConfiguration.FindAsync(s => s.Id == id);
            return result.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }

        public async Task<IEnumerable<WorkFlowConfigurationModel>> GetWorkFlowConfigurationsByWorkflowIdAsync(int id)
        {
            var result = await _workFlowConfiguration.FindAllAsync(s => s.MasterWorkFlowId == id);
            return result.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }

        public async Task<IPagedList<WorkFlowConfigurationModel>> GetPagedQueryWorkFlowConfigurationsAsync(int pageNumber, int pageSize)
        {
            var result = await _workFlowConfiguration.ExecuteQueryAsyc<WorkFlowConfigurationModel>("SELECT wfc.*,org.[Name] AS [OrgRoleName],wf.[Name] AS [MasterWorkFlowName] FROM [dbo].[WorkFlowConfigurations] AS wfc INNER JOIN[dbo].[WorkFlows] AS wf ON wfc.MasterWorkFlowId = wf.Id INNER JOIN[dbo].[OrganizationRoles] AS org ON wfc.OrgRoleId = org.Id");
            return await result.AsQueryable().ToPagedListAsync(pageNumber, pageSize);

        }

        public async Task<WorkFlowConfigurationModel> SaveAsync(WorkFlowConfigurationModel model)
        {
            var example = model.ToMap<WorkFlowConfigurationModel, WorkFlowConfiguration>();
            var result = await _workFlowConfiguration.CreateOrUpdateAsync(example);

            try
            {
                if (result.MasterWorkFlowId > 0)
                {
                    var wfTypeId = await _workFlowConfiguration.GetAllInclude(a => a.WorkFlow).FirstOrDefaultAsync();
                    if (wfTypeId != null)
                    {
                        var wfType = await _workflowtype.FindAsync(a => a.Id == wfTypeId.WorkFlow.WorkflowType);
                        if (wfType != null)
                        {
                            wfType.IsWorkflowConfigAvailable = true;
                            var res = await _workflowtype.CreateOrUpdateAsync(wfType);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return result.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }

        public async Task<WorkFlowConfigurationModel> UpdateAsync(WorkFlowConfigurationModel model)
        {
            var example = model.ToMap<WorkFlowConfigurationModel, WorkFlowConfiguration>();
            var result = await _workFlowConfiguration.UpdateAsync(example);
            return result.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
        }


    }
}

