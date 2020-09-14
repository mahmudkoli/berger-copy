import { Component, OnInit } from '@angular/core';
import { WorkFlowService } from 'src/app/Shared/Services/Workflow/work-flow.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { WorkflowconfigurationService } from 'src/app/Shared/Services/Workflow/workflowconfiguration.service';
import { WorkFlow } from 'src/app/Shared/Entity/WorkFlows/work-flow';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { WorkflowTypes } from 'src/app/Shared/Enums/WorkflowTypes';

@Component({
  selector: 'app-work-flow-treeview',
  templateUrl: './work-flow-treeview.component.html',
  styleUrls: ['./work-flow-treeview.component.css']
})
export class WorkFlowTreeviewComponent implements OnInit {


  heading = 'Create A New Work Flow';
  subheading = '';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
   enumWorkFlowType  : MapObject[] = WorkflowTypes.WorkflowType;
   tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
   tosterMsgError: string = "Something went wrong!";

   workFlowList: WorkFlow[] = [];

  constructor(
    private router: Router,
    private workFlowService: WorkFlowService,
    private alertService: AlertService,
    private configService : WorkflowconfigurationService
  ) { }

  ngOnInit() {

    this.getWorkFLows();
  }

//treeview
  getWorkFLows() {
    this.alertService.fnLoading(true);
    this.workFlowService.getWorkflowListWithConfig().subscribe(
      (res: any) => {
        this.workFlowList = res.data;
        console.log(this.workFlowList);
        this.workFlowList.forEach(s => s.checked = true);
        
        
      },
      (error) => {
        console.log(error);
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }


}
