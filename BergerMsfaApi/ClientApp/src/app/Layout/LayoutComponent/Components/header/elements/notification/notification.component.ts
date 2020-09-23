import { Component, OnInit } from '@angular/core';
import { WorkflowLog } from 'src/app/Shared/Entity/WorkFlows/workflow-log';
import { Router } from '@angular/router';
import { WorkflowLogService } from 'src/app/Shared/Services/Workflow/workflow-log.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { trigger, state, animate, transition, keyframes, style } from '@angular/animations';
import { WorkflowLogHistory } from 'src/app/Shared/Entity/WorkFlows/workflow-log-history';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalNotificationComponent } from '../../modal-notification/modal-notification.component';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';
import { WorkflowLogHistoryService } from 'src/app/Shared/Services/Workflow/workflow-log-history.service';
import { WorkflowStatusEnumLabel, WorkflowStatusEnum } from 'src/app/Shared/Enums/workflowStatusEnum';



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

  constructor(
    private router: Router,
    private workflowLogService: WorkflowLogService,
    private workflowLogHistoryService: WorkflowLogHistoryService,
    private alertService: AlertService,
    private modalService: NgbModal,
  
  ) { }

  ngOnInit() {
    // this.getWorkflowLogForCurrentUser();
    // this.getWorkflowLogHistoryForCurrentUser();
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
        console.log(this.workflowLogList);
      },
      (error) => {
        console.log(error);
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
        console.log(this.workflowLogHistoryList);
      },
      (error) => {
        console.log(error);
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
