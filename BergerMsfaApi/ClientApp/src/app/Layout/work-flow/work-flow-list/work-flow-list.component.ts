import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { WorkFlowService } from 'src/app/Shared/Services/Workflow/work-flow.service';
import { Status } from 'src/app/Shared/Enums/status';
import { WorkFlow } from 'src/app/Shared/Entity/WorkFlows/work-flow';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-work-flow-list',
  templateUrl: './work-flow-list.component.html',
  styleUrls: ['./work-flow-list.component.css']
})
export class WorkFlowListComponent implements OnInit {

  heading = 'Create A New Work Flow';
  subheading = '';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
   enumWorkFlowType  : MapObject[] = Enums.WorkflowType;
   workflowType : any;
   enumStatus = Status;
   tosterMsgDltSuccess: string = "Record has been deleted successfully.";
   tosterMsgError: string = "Something went wrong!";
   permissionGroup: PermissionGroup = new PermissionGroup();


  constructor(
    private router: Router,
    private workFlowService: WorkFlowService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService
  ) {
    this.initPermissionGroup();

   }

  workFlowList: WorkFlow[] = [];
  listOfWorkflowType: any;

  async ngOnInit() {
    await this.getWorkflowType();
   
    
  }

  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
    this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
}

  createWorkFLow() {
    this.router.navigate(['/work-flow/work-flow-add']);
  }

  getWorkFLows() {
    this.alertService.fnLoading(true);
    this.workFlowService.getWorkFLowList().subscribe(
      (res: any) => {
        this.workFlowList = res.data;
        this.workFlowList.forEach(s => {
          s.status2 = this.enumStatus[s.status];


          var data = this.listOfWorkflowType.filter(k => k.id == s.workflowType)[0];
          if(data){
            s.workflowTypeName = data.workflowTypeName;
          }

           
          
         });
        
        
      },
      (error) => {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }

 async getWorkflowType() {
    this.workFlowService.getWorflowType().subscribe((result: any) => {
      this.listOfWorkflowType = result.data;
      console.log(this.listOfWorkflowType);
      this.getWorkFLows();

    });
  }

  edit(id: number) {
    this.router.navigate([`/work-flow/work-flow-add/${id}`]);
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure you want to delete this item?",
      () => {
        this.alertService.fnLoading(true);
        this.workFlowService.deleteWorkFlow(id).subscribe(
          (succ: any) => {
            this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
            this.getWorkFLows();
          },
          (error) => {
            this.alertService.fnLoading(false);
            this.alertService.tosterDanger(this.tosterMsgError);
          },
          () => this.alertService.fnLoading(false));
      }, () => { });
  }

  public ptableSettings = {
    tableID: "workFlow-table",
    tableClass: "table-responsive",
    tableName: 'Workflow Definition List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      
      { headerName: 'Code', width: '20%', internalName: 'code', sort: true, type: "" },
      { headerName: 'Name', width: '40%', internalName: 'name', sort: true, type: "" },
      // { headerName: 'Action', width: '20%', internalName: 'action', sort: true, type: "" },
      { headerName: 'Workflow Type', width: '20%', internalName: 'workflowTypeName', sort: true, type: "" },
      { headerName: 'Step', width: '10%', internalName: 'workflowStep', sort: true, type: "" },
      { headerName: 'Status', width: '10%', internalName: 'status2', sort: true, type: "" }
      
     
      
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledEditBtn: true,
    enabledDeleteBtn: true,
    enabledColumnFilter: true, 
    enabledRecordCreateBtn: true,
  };

  public fnCustomTrigger(event) {
    console.log("custom  click: ", event);

    if (event.action == "new-record") {
      this.createWorkFLow();
    }
    else if (event.action == "edit-item") {
      this.edit(event.record.id);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
  }

}
