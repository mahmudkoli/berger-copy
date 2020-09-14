import { Component, OnInit } from '@angular/core';
import { WorkFlowService } from 'src/app/Shared/Services/Workflow/work-flow.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { FormGroup } from '@angular/forms';
import { Status } from 'src/app/Shared/Enums/status';
import { WorkFlow } from 'src/app/Shared/Entity/WorkFlows/work-flow';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';

@Component({
  selector: 'app-work-flow-add',
  templateUrl: './work-flow-add.component.html',
  styleUrls: ['./work-flow-add.component.css']
})
export class WorkFlowAddComponent implements OnInit {

  public form: FormGroup;

  constructor(private workFlowService: WorkFlowService,
    private router: Router,
    private alertService: AlertService,
    private route: ActivatedRoute) { }

  async ngOnInit() {

    await this.getWorkflowType();
    this.createForm();

    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      let workFlowId = this.route.snapshot.params.id;
      this.getWorkflow(workFlowId);
      
    }
    
  }

  workFlowModel: WorkFlow;
  listOfWorkflowType: any;

  statusOptions: {};
  keys: string[];
  minNum = 15;
  maxNum = 50;
  checkWokrflowStep: boolean = false;

  getWorkflowType() {
    this.workFlowService.getWorflowType().subscribe((result: any) => {
      this.listOfWorkflowType = result.data;

    });
  }

  getWorkflow(workFlowId) {
    this.workFlowService.getWorkflowById(workFlowId).subscribe(
      (result: any) => {
       
        this.editForm(result.data);
      },
      (err: any) => console.log(err)
    );
  };

  editForm(workFlow: any) {
    this.workFlowModel.id = workFlow.id;
    this.workFlowModel.name = workFlow.name;
    this.workFlowModel.action = workFlow.action;
    this.workFlowModel.code = workFlow.code;
    this.workFlowModel.workflowStep = workFlow.workflowStep;
    this.workFlowModel.workflowType = workFlow.workflowType;
    this.workFlowModel.status = workFlow.status;

  }

  createForm() {

    this.statusOptions = Status;
    this.keys = Object.keys(this.statusOptions).filter(k => !isNaN(Number(k)));
    this.workFlowModel = new WorkFlow();
   

  }

  saveWorkFlow(model: WorkFlow) {
    if(model.workflowStep < 0 || model.workflowStep > 5){

      this.checkWokrflowStep = true;
      debugger;
    }
    else{
      this.checkWokrflowStep = false;
      model.code = model.code.trim();
    console.log("Workflow model: ", model);
    this.workFlowService.postWorkFlow(model).subscribe((res) => {
     
      this.router.navigate(['/work-flow/work-flow-list']).then(() => {
        this.alertService.titleTosterSuccess("Record has been saved successfully.");
      });
    },
      (error) => {
       
        this.displayError(error);
      }, () => this.alertService.fnLoading(false)


    );

    }
    
  }

  workFlowList() {
    this.router.navigate(['/work-flow/work-flow-list']);
  }

  displayError(errorDetails: any) {
    // this.alertService.fnLoading(false);
   
    let errList = errorDetails.error.errors;
    if (errList.length) {
      
      this.alertService.tosterDanger(errList[0].errorList[0]);
    } else {
      this.alertService.tosterDanger(errorDetails.error.msg);
    }
  }

  validateWorkStep() {

    if (this.workFlowModel.workflowStep > 4 || this.workFlowModel.workflowStep < 0) {
      this.checkWokrflowStep = true;
    }
    else{
      this.checkWokrflowStep = false;
    }
   
  }


}
