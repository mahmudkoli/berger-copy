import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { WorkflowLog } from 'src/app/Shared/Entity/WorkFlows/workflow-log';
import { WorkflowLogHistory } from 'src/app/Shared/Entity/WorkFlows/workflow-log-history';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { WorkflowStatusEnum, WorkflowStatusEnumLabel } from 'src/app/Shared/Enums/workflowStatusEnum';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { NotificationService } from 'src/app/Shared/Services/Notification/Notification.service';
import { WorkflowLogHistoryService } from 'src/app/Shared/Services/Workflow/workflow-log-history.service';
import { WorkflowLogService } from 'src/app/Shared/Services/Workflow/workflow-log.service';
import { ModalNotificationComponent } from '../../modal-notification/modal-notification.component';



@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css'],
  animations: [
    trigger('slideInOutAnimation', [
      transition(':enter', [
          animate(".6s", keyframes([
            style({
               opacity: '0',
               transform: 'translateX(-30px)'
           }),
            style({
                opacity: '0.5',
                transform: 'translateX(15px)'
              }),
            style({
               opacity: '1',
               transform:' translateX(0)'
              })
          ]))
      ]),
      // transition(':leave', [
      //     animate('.6s ease-in-out', style({
      //         right: '-400%',
      //         backgroundColor: 'rgba(0, 0, 0, 0)'
      //     }))
      // ])
  ])
  ]
})
export class NotificationComponent implements OnInit {


   tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
   tosterMsgError: string = "Something went wrong!";

   workflowLogList: WorkflowLog[] = [];
   workflowLogHistoryList: WorkflowLogHistory[] = [];
   pendingWorkflowCount : number = 0;
   closeResult: string;
   workFlowStatusEnumLabel : MapObject[] =  WorkflowStatusEnumLabel.workflowStatusEnumLabel;
   workFlowStatusEnum = WorkflowStatusEnum;
  lstList=[];
  lstDealerOpening=[];
  lstJourneyPlan=[]
  constructor(
    private router: Router,
    private workflowLogService: WorkflowLogService,
    private workflowLogHistoryService: WorkflowLogHistoryService,
    private alertService: AlertService,
    private modalService: NgbModal,
    private notificationService: NotificationService

  ) { }

  ngOnInit() {
    // this.getWorkflowLogForCurrentUser();
    // this.getWorkflowLogHistoryForCurrentUser();
    this.notificationService.getJourneyPlanList().subscribe((res:any)=>{
      this.lstDealerOpening=res.notificationForDealerOpningModel
      this.lstJourneyPlan=res.notificationForJourneyPlan
      this.pendingWorkflowCount=res.totalNoification
    })
  }

  getStatusText(status) {
    const list = this.workFlowStatusEnumLabel.filter(k => k.id == status);
   return list.length > 0 ? list[0].label : 'NULL';
 }

  getWorkflowLogForCurrentUser() {
    this.alertService.fnLoading(true);
    this.workflowLogService.getWorkflowLogForCurrentUser(this.workFlowStatusEnum.Pending,1,10).subscribe(
      (res: any) => {
        this.workflowLogList = res.data;
        this.pendingWorkflowCount = res.data.length > 0 ? res.data[0].pendingWorkflowCount : 0;
      },
      (error) => {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }


  getWorkflowLogHistoryForCurrentUser() {
    this.alertService.fnLoading(true);
    this.workflowLogHistoryService.getWorkflowLogHistoryForCurrentUser(1, 10).subscribe(
      (res: any) => {
        this.workflowLogHistoryList = res.data;
      },
      (error) => {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }

  fnRouteNotification(){
    this.router.navigate(['/notification/notification-details']);
  }



  openWorkflowLogModal(workflowLog, status) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false
    };
    this.pendingWorkflowCount=0;

    const modalRef = this.modalService.open(ModalNotificationComponent, ngbModalOptions);
    modalRef.componentInstance.workflowLogModel = workflowLog;
    modalRef.componentInstance.workflowStatus = status;

    modalRef.result.then((result) => {
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
