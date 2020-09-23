import { Component, OnInit, Input } from '@angular/core';
import { WorkflowLogHistory } from 'src/app/Shared/Entity/WorkFlows/workflow-log-history';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { WorkflowLogHistoryService } from 'src/app/Shared/Services/Workflow/workflow-log-history.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { WorkflowLog } from 'src/app/Shared/Entity/WorkFlows/workflow-log';
import { WorkflowStatusEnumLabel, WorkflowStatusEnum } from 'src/app/Shared/Enums/workflowStatusEnum';

@Component({
  selector: 'app-modal-notification',
  templateUrl: './modal-notification.component.html',
  styleUrls: ['./modal-notification.component.css']
})
export class ModalNotificationComponent implements OnInit {

  @Input() workflowLogModel: WorkflowLog;
  @Input() workflowStatus: number;
  workflowLogHistory: WorkflowLogHistory;
  

  workFlowStatusEnumLabel : MapObject[] =  WorkflowStatusEnumLabel.workflowStatusEnumLabel;
  workFlowStatusEnum = WorkflowStatusEnum;
  tosterMsgError: string = "Something went wrong";

  constructor(
    public activeModal: NgbActiveModal,
    public workflowLogHistoryService: WorkflowLogHistoryService,
    private alertService: AlertService
  ) {
      this.workflowLogHistory = new WorkflowLogHistory();
   }

  ngOnInit() {
    this.workflowLogHistory.workflowLogId = this.workflowLogModel.id;
    this.workflowLogHistory.workflowStatus = this.workflowStatus;
    debugger;
  }

  getStatusText(status) {
    const list = this.workFlowStatusEnumLabel.filter(k => k.id == status);
   return list.length > 0 ? list[0].label : 'NULL';
 }


  create() {
    
  }

  update() {
    this.workflowLogHistoryService.postWorkflowLogHistory(this.workflowLogHistory).subscribe(
      (res: any) => {
        console.log(res.data);
        this.activeModal.close("Workflow Log History updated");
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  submit() {
    
    this.workflowLogHistoryService.postWorkflowLogHistory(this.workflowLogHistory).subscribe(
      (res: any) => {
        console.log(res.data);
        this.activeModal.close("Workflow Log updated");
        this.alertService.titleTosterSuccess("Workflow Log updated.");
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  showError(msg: string = null) {
    this.activeModal.close(msg ? msg : this.tosterMsgError);
  }

}
