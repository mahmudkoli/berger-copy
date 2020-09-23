using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Organizations;
using Berger.Data.MsfaEntity.WorkFlows;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.WorkFlows;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.WorkFlows.Interfaces;

namespace BergerMsfaApi.Services.WorkFlows.Implementation
{
    public class WorkFlowService : IWorkFlowService
    {
        private readonly IRepository<WorkFlow> _workFlow;
        private readonly IRepository<WorkFlowConfiguration> _workflowConfig;
        private readonly IRepository<OrganizationRole> _organizationRole;
        private readonly IRepository<WorkFlowType> _workflowtype;


        public WorkFlowService(IRepository<WorkFlow> workflow, IRepository<WorkFlowConfiguration> workFlowConfiguration, IRepository<OrganizationRole> organizationRole, IRepository<WorkFlowType> workflowType)
        {
            _workFlow = workflow;
            _workflowConfig = workFlowConfiguration;
            _organizationRole = organizationRole;
            _workflowtype = workflowType;
        }


        public async Task<int> DeleteAsync(int id)
        {
            var result = await _workFlow.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsCodeExistAsync(string code, int id)
        {
            var result = id <= 0
                ? await _workFlow.IsExistAsync(s => s.Code == code)
                : await _workFlow.IsExistAsync(s => s.Code == code && s.Id != id);

            return result;
        }

        public async Task<bool> IsWorkflowTypeExistAsync(int workflowTypeId, int id)
        {
            var result = id <= 0
                ? await _workFlow.IsExistAsync(s => s.WorkflowType == workflowTypeId)
                : await _workFlow.IsExistAsync(s => s.WorkflowType == workflowTypeId && s.Id != id);

            return result;
        }
        public async Task<WorkFlowModel> GetWorkFlowAsync(int id)
        {
            var result = await _workFlow.FindAsync(s => s.Id == id);
            return result.ToMap<WorkFlow, WorkFlowModel>();
        }



        public async Task<IEnumerable<WorkFlowModel>> GetWorkFlowsAsync()
        {
            var result = await _workFlow.GetAllAsync();
            result = result.OrderByDescending(a => a.CreatedTime);
            return result.ToMap<WorkFlow, WorkFlowModel>();
        }

        public async Task<IEnumerable<WorkflowTypeModel>> GetWorkFlowTypeAsync()
        {
            var result = await _workflowtype.FindAllAsync(w => w.IsActive == true);
            result.Select(a => new { a.Id, a.WorkflowTypeName });
            return result.ToMap<WorkFlowType, WorkflowTypeModel>();
        }

        public async Task<IEnumerable<WorkFlowModel>> GetWorkFlowsWithConfigAsync()
        {
            var result = await _workFlow.GetAllAsync();

            var result2 = result.ToMap<WorkFlow, WorkFlowModel>();

            var ConfigListData = await _workflowConfig.GetAllAsync();
            var LocalConfigListData = ConfigListData.ToMap<WorkFlowConfiguration, WorkFlowConfigurationModel>();
            var OrganizationRoleData = await _organizationRole.GetAllAsync();
            

            foreach (var item in result2)
            {
                var configList = LocalConfigListData.Where(c => c.MasterWorkFlowId == item.Id).OrderBy(m => m.sequence).ToList();

                foreach (var config in configList)
                {
                    var organizationRole = OrganizationRoleData.Where(o => o.Id == config.OrgRoleId).FirstOrDefault();

                    config.OrgRoleName = organizationRole.Name;
                }

                item.ConfigList = configList;


            }



            return result2;
        }

        public async Task<WorkFlowModel> SaveAsync(WorkFlowModel model)
        {
            var example = model.ToMap<WorkFlowModel, WorkFlow>();
            var result = await _workFlow.CreateOrUpdateAsync(example);
            if (model.WorkflowType >  0)
            {
                var wfType = await _workflowtype.FirstOrDefaultAsync(a => a.Id == model.WorkflowType);
                if (wfType != null)
                {
                    wfType.IsWorkflowDefAvailable = true;
                    var res = await _workflowtype.CreateOrUpdateAsync(wfType);
                }
                
            }
            return result.ToMap<WorkFlow, WorkFlowModel>();
        }


    }
}
