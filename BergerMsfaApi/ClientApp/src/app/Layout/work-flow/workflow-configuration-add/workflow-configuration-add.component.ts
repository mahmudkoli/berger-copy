import { Component, OnInit } from '@angular/core';
import { WorkflowconfigurationService } from "src/app/Shared/Services/Workflow/workflowconfiguration.service";
import { WorkFlowConfiguration } from "src/app/Shared/Entity/WorkFlows/workflowconfiguration";
import { ActivatedRoute, Router } from '@angular/router';
import { WorkFlowService } from "../../../Shared/Services/Workflow/work-flow.service";
import { OrganizationRoleService } from "../../../Shared/Services/Workflow/organizationrole.service";
import { ModeOfApprovals } from "../../../Shared/Enums/modeOfApproval";
import { ApprovalStatuses } from "../../../Shared/Enums/approvalStatus";
import { RejectedStatuses } from "../../../Shared/Enums/rejectedStatus";
import { NotificationStatuses } from "../../../Shared/Enums/notificationStatus";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { Status } from "../../../Shared/Enums/status";
import { OrganizationRole } from "../../../Shared/Entity/Organizations/orgrole";
import { WorkFlow } from 'src/app/Shared/Entity/WorkFlows/work-flow';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";


@Component({
    selector: 'app-workflow-configuration-add',
    templateUrl: './workflow-configuration-add.component.html',
    styleUrls: ['./workflow-configuration-add.component.css']
})
export class WorkflowConfigurationAddComponent implements OnInit {

    public workFlowConfigurationModel: WorkFlowConfiguration = new WorkFlowConfiguration();
    public workFlowList: WorkFlow[] = [];
    public organizationRoleList: OrganizationRole[] = [];

    modeOfApprovalOptions: MapObject[] = ModeOfApprovals.modeOfApproval;
    approvalStatusOptions: MapObject[] = ApprovalStatuses.approvalStatus;
    rejectedStatusOptions: MapObject[] = RejectedStatuses.rejectedStatus;
    notificationStatusOptions: MapObject[] = NotificationStatuses.notificationStatus;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;

    constructor(private alertService: AlertService, private route: ActivatedRoute, private workflowconfigurationService: WorkflowconfigurationService, private workFlowService: WorkFlowService, private organizationRoleService: OrganizationRoleService,private router: Router) { }

    ngOnInit() {
        this.fnGetWorkFlowList();
        this.fnGetOrganizationRoleList();

        this.createForm();


        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            
            let workflowConfigId = this.route.snapshot.params.id;
            this.getWorkflowConfiguration(workflowConfigId);
        }
    }

    createForm() {
    }

    public fnRouteWorkFlowConfigurationList() {
        this.router.navigate(['/work-flow/workflow-configuration-list']);
    }

    private getWorkflowConfiguration(workflowConfigId) {
        this.workflowconfigurationService.getWorkFlowConfigurationById(workflowConfigId).subscribe(
            (result: any) => {
               
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(workFlowConfiguration: WorkFlowConfiguration) {
        this.workFlowConfigurationModel.id = workFlowConfiguration.id;
        this.workFlowConfigurationModel.orgRoleId = workFlowConfiguration.orgRoleId;
        this.workFlowConfigurationModel.masterWorkFlowId = workFlowConfiguration.masterWorkFlowId;
        this.workFlowConfigurationModel.modeOfApproval = workFlowConfiguration.modeOfApproval;
        this.workFlowConfigurationModel.approvalStatus = workFlowConfiguration.approvalStatus;
        this.workFlowConfigurationModel.rejectedStatus = workFlowConfiguration.rejectedStatus;
        this.workFlowConfigurationModel.notificationStatus = workFlowConfiguration.notificationStatus;
        this.workFlowConfigurationModel.status = workFlowConfiguration.status;
        this.workFlowConfigurationModel.sequence = workFlowConfiguration.sequence;
        
    }

    public fnSaveWorkFlowConfiguration() {
        this.workFlowConfigurationModel.id == 0 ? this.insertWorkflowConfiguration(this.workFlowConfigurationModel) : this.updateWorkflowConfiguration(this.workFlowConfigurationModel);
    }

    private insertWorkflowConfiguration(model: WorkFlowConfiguration) {
        this.workflowconfigurationService.postWorkFlowConfiguration(model).subscribe(res => {
            
            this.router.navigate(['/work-flow/workflow-configuration-list']).then(() => {
                this.alertService.tosterSuccess("WorkFlow Configuration has been created successfully.");
            });
        },
            (error) => {
                
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateWorkflowConfiguration(model: WorkFlowConfiguration) {
        this.workflowconfigurationService.putWorkFlowConfiguration(model).subscribe(res => {
           
            this.router.navigate(['/work-flow/workflow-configuration-list']).then(() => {
                this.alertService.tosterSuccess("WorkFlow Configuration has been edited successfully.");
            });
        },
            (error) => {
               
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
       
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

    private fnGetWorkFlowList() {
        this.workFlowService.getWorkFLowList().subscribe((res: any) => {
            this.workFlowList = res.data;
           
        });
    }

    private fnGetOrganizationRoleList() {
        this.organizationRoleService.getOrganizationRoleList().subscribe((res: any) => {
            this.organizationRoleList = res.data.model;
           
        });
    }
}
