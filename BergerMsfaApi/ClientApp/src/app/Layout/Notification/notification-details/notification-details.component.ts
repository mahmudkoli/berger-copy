import { Component, OnInit } from '@angular/core';
import { WorkflowLog } from 'src/app/Shared/Entity/WorkFlows/workflow-log';
import { Router } from '@angular/router';
import { WorkflowLogService } from 'src/app/Shared/Services/Workflow/workflow-log.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalNotificationComponent } from '../../LayoutComponent/Components/header/modal-notification/modal-notification.component';
import { WorkflowLogHistoryService } from 'src/app/Shared/Services/Workflow/workflow-log-history.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';
import { WorkflowLogHistory } from 'src/app/Shared/Entity/WorkFlows/workflow-log-history';
import { finalize } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import { WorkflowStatusEnum, WorkflowStatusEnumLabel } from 'src/app/Shared/Enums/workflowStatusEnum';
import { NotificationService } from 'src/app/Shared/Services/Notification/Notification.service';

@Component({
  selector: 'app-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.css']
})
export class NotificationDetailsComponent implements OnInit {

  tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
  tosterMsgError: string = "Something went wrong!";

  workflowLogList: WorkflowLog[] = [];
  workflowLogHistoryList: WorkflowLogHistory[] = [];
  // historyCount : number = 0;
  closeResult: string;
  pendingWorkflowCount : number = 0;
  workFlowStatusEnumLabel : MapObject[] =  WorkflowStatusEnumLabel.workflowStatusEnumLabel;
  workFlowStatusEnum = WorkflowStatusEnum;
  tabOpen: number = 1;
  lstList=[];

 constructor(
   private router: Router,
   private workflowLogService: WorkflowLogService,
   private workflowLogHistoryService: WorkflowLogHistoryService,
   private alertService: AlertService,
   private modalService: NgbModal,
   private notificationService: NotificationService
 
 ) { }

 ngOnInit() {

  //  this.getWorkflowLogForCurrentUser();
  //  this.getWorkflowLogHistoryForCurrentUser();
  this.getWorkflowLogWithHistoryForCurrentUser();
  this.notificationService.getJourneyPlanList().subscribe((res:any)=>{
    this.lstList=res;
  })
 }


 getStatusText(status) {
   const list = this.workFlowStatusEnumLabel.filter(k => k.id == status);
  return list.length > 0 ? list[0].label : 'NULL';
}

getWorkflowLogWithHistoryForCurrentUser(){
  this.alertService.fnLoading(true);
  forkJoin(
     this.workflowLogService.getWorkflowLogForCurrentUser(null,1,100000),
     this.workflowLogHistoryService.getWorkflowLogHistoryForCurrentUser(1, 100000)
  )
   .pipe(
     finalize(() => {
       this.alertService.fnLoading(false);
     })
   ).subscribe((res: any) => {
    // workflow log
      this.workflowLogList = res[0].data;
      this.pendingWorkflowCount = res[0].data.length > 0 ? res[0].data[0].pendingWorkflowCount : 0;
      console.log('workflow logs', this.workflowLogList);

      // workflow log history
      this.workflowLogHistoryList = res[1].data;
      console.log('workflow log histories', this.workflowLogHistoryList);
    });
}

 getWorkflowLogForCurrentUser() {
   this.alertService.fnLoading(true);
   this.workflowLogService.getWorkflowLogForCurrentUser(null,1,100000)
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res: any) => {
       this.workflowLogList = res.data;
       this.pendingWorkflowCount = res.data.length > 0 ? res.data[0].pendingWorkflowCount : 0;
       console.log(this.workflowLogList);
      //  this.workflowLogWithHistoryList.forEach(s => {

        //  this.historyCount += s.pendingWorkflowCount;
        // console.log(s.historyCount);

      //  });
           
     });
 }
 
 getWorkflowLogHistoryForCurrentUser() {
  this.alertService.fnLoading(true);
  this.workflowLogHistoryService.getWorkflowLogHistoryForCurrentUser(1, 100000)
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res: any) => {
      this.workflowLogHistoryList = res.data;
      console.log(this.workflowLogHistoryList);
    });
}

 fnRouteNotification(){
   this.router.navigate(['/notification/notification-details']);
 }

 openWorkflowLogModal(workflowLog, status) {
  let ngbModalOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false
  };
  const modalRef = this.modalService.open(ModalNotificationComponent, ngbModalOptions);
  modalRef.componentInstance.workflowLogModel = workflowLog;
  modalRef.componentInstance.workflowStatus = status;

  modalRef.result.then((result) => {
    console.log(result);
    this.closeResult = `Closed with: ${result}`;
   // this.getMenus();
   this.getWorkflowLogForCurrentUser();
   this.getWorkflowLogHistoryForCurrentUser();
  },
    (reason) => {
      console.log(reason);
    });
}

}
