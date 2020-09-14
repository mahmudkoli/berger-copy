import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkflowconfigurationService } from "src/app/Shared/Services/Workflow/workflowconfiguration.service";
import { WorkFlowConfiguration } from "src/app/Shared/Entity/WorkFlows/workflowconfiguration";
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Status } from 'src/app/Shared/Enums/status';
import { YesNo } from 'src/app/Shared/Enums/yesno';
import { ModeOfApprovals } from "src/app/Shared/Enums/modeOfApproval";
import { ApprovalStatuses } from "src/app/Shared/Enums/approvalStatus";
import { NotificationStatuses } from "src/app/Shared/Enums/notificationStatus";
import { RejectedStatuses } from "src/app/Shared/Enums/rejectedStatus";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { kMaxLength } from 'buffer';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
    selector: 'app-workflow-configuration-list',
    templateUrl: './workflow-configuration-list.component.html',
    styleUrls: ['./workflow-configuration-list.component.css']
})


export class WorkflowConfigurationListComponent implements OnInit {

    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    modeOfApprovalOptions: MapObject[] = ModeOfApprovals.modeOfApproval;
    approvalStatusOptions: MapObject[] = ApprovalStatuses.approvalStatus;
    rejectedStatusOptions: MapObject[] = RejectedStatuses.rejectedStatus;
    notificationStatusOptions: MapObject[] = NotificationStatuses.notificationStatus;

    mapObject : MapObject;
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private workflowconfigurationService: WorkflowconfigurationService,
         private alertService: AlertService,
         private router: Router,
         private activatedRoute: ActivatedRoute,
         private activityPermissionService: ActivityPermissionService) {
            this.initPermissionGroup();

    }

    ngOnInit() {
        this.fnGetWorkflowconfigurationList();
    }

    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    public workflowconfigurationList: WorkFlowConfiguration[] = [];
    private fnGetWorkflowconfigurationList() {
        this.alertService.fnLoading(true);
        this.workflowconfigurationService.getWorkFlowConfigurationList().subscribe(
            (res) => {
                let workflowconfigurationsData = res.data.model || [];
                workflowconfigurationsData.forEach(obj => {

                   
                    this.mapObject = this.enumStatusTypes.filter(k => k.id == obj.status)[0];

                    if(this.mapObject != null)
                    {
                        obj.statusText = this.mapObject.label;
                        
                    }
                   
                    this.mapObject = this.modeOfApprovalOptions.filter(k => k.id == obj.modeOfApproval)[0];
                    if(this.mapObject != null)
                    {
                        obj.enumOptions1 = this.mapObject.label;
                    }
                    
                    this.mapObject = this.approvalStatusOptions.filter(k => k.id == obj.approvalStatus)[0];
                    if(this.mapObject != null)
                    {
                        obj.enumOptions2 = this.mapObject.label;
                    }
                    
                    this.mapObject  = this.rejectedStatusOptions.filter(k => k.id == obj.rejectedStatus)[0];
                    if(this.mapObject != null){
                        obj.enumOptions3 = this.mapObject.label;
                    }

        
                   
                    this.mapObject  = this.notificationStatusOptions.filter(k => k.id == obj.notificationStatus)[0];
                    if(this.mapObject != null)
                    {
                        obj.enumOptions4 = this.mapObject.label;
                    }
                   
                });
                this.workflowconfigurationList = workflowconfigurationsData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    private fnRouteAddPosmProd() {
        this.router.navigate(['/work-flow/workflow-configuration-add']);
    }

    private edit(id: number) {
        this.router.navigate(['/work-flow/workflow-configuration-add/' + id]);
    }

    private delete(id: number) {
       
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.workflowconfigurationService.deleteWorkFlowConfiguration(id).subscribe(
                (res: any) => {
                  
                    this.alertService.tosterSuccess("Workflow Configuration has been deleted successfully.");
                    this.fnGetWorkflowconfigurationList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public fnPtableCellClick(event) {
        console.log("cell click: ");
    }

    public ptableSettings = {
        tableID: "Products-table",
        tableClass: "table table-border ",
        tableName: 'WorkFlow Configuration List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Organization Role ', width: '14%', internalName: 'orgRoleName', sort: true, type: "" },
            { headerName: 'Master WorkFlow ', width: '14%', internalName: 'masterWorkFlowName', sort: true, type: "" },
            { headerName: 'Mode of Approval ', width: '14%', internalName: 'enumOptions1', sort: true, type: "" },
            { headerName: 'Approval Status ', width: '14%', internalName: 'enumOptions2', sort: true, type: "" },
            { headerName: 'Notification Status', width: '14%', internalName: 'enumOptions3', sort: true, type: "" },
            { headerName: 'Rejected Status', width: '14%', internalName: 'enumOptions4', sort: true, type: "" },
            { headerName: 'Status', width: '9%', internalName: 'statusText', sort: true, type: "" },
            { headerName: 'Sequence', width: '5%', internalName: 'sequence', sort: false, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledEditBtn: true,
        enabledDeleteBtn: true,
        //enabledEditDeleteBtn: true,
        // enabledCellClick: true,
        enabledColumnFilter: true,
        // enabledDataLength:true,
        // enabledColumnResize:true,
        // enabledReflow:true,
        // enabledPdfDownload:true,
        // enabledExcelDownload:true,
        // enabledPrint:true,
        // enabledColumnSetting:true,
        enabledRecordCreateBtn: true,
        // enabledTotal:true,
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.fnRouteAddPosmProd();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }

}
