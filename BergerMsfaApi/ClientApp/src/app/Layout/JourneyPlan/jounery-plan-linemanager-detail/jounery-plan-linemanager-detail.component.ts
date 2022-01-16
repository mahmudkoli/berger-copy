
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Component, OnInit } from '@angular/core';
import { PlanStatus } from '../../../Shared/Enums/PlanStatus';
import { JourneyPlanStatus } from '../../../Shared/Entity/JourneyPlan/JourneyPlanStatus';


@Component({
  selector: 'app-jounery-plan-linemanager-detail',
  templateUrl: './jounery-plan-linemanager-detail.component.html',
  styleUrls: ['./jounery-plan-linemanager-detail.component.css']
})
export class JouneryPlanLinemanagerDetailComponent implements OnInit {

    PlanStatusEnum = PlanStatus;
    PlanStatusEnumNotEdited = PlanStatusNotEdited;
    statusKeys: any[] = [];
    journeyPlanStatus: JourneyPlanStatus = new JourneyPlanStatus();
    journeyPlan: any;

    constructor(
        public alertService: AlertService,
        public formatter: NgbDateParserFormatter,
        private route: ActivatedRoute,
        private journeyPlanService: JourneyPlanService,
        private router: Router) { }

    ngOnInit() {
        this.statusKeys = Object.keys(this.PlanStatusEnumNotEdited).filter(k => !isNaN(Number(k)));
        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getJourneyPlanById(id);
        }

    }
    

    private getJourneyPlanById(id) {
        this.alertService.fnLoading(true);
        this.journeyPlanService.getJourneyPlanDetailById(id).subscribe(
            (result: any) => {
                // debugger;
                this.journeyPlan = result.data;
               // this.journeyPlanStatus.comment = this.journeyPlan.comment;

                if (this.compareDate(this.journeyPlan.planDate)) {
                    this.showRejectedBtn = true;
                    this.showApprovedBtn = true;
                    if ((this.journeyPlan.planStatus) as PlanStatus == PlanStatus.ChangeRequested) {
                        this.showRejectedBtn = false;
                        this.showApprovedBtn = true;
                    }
                    if ((this.journeyPlan.planStatus) as PlanStatus == PlanStatus.Approved) {
                        this.showStatusBtn = false;
                        this.showRejectedBtn = false;
                        this.showApprovedBtn = false;
                    }
                    else if ((this.journeyPlan.planStatus) as PlanStatus == PlanStatus.Edited || (this.journeyPlan.planStatus) as PlanStatus == PlanStatus.Pending) {
                        this.showRejectedBtn = true;
                        this.showApprovedBtn = true;
                    }
                    else
                        this.showStatusBtn = true;
                }
                else {
                    this.showRejectedBtn = false;
                    this.showApprovedBtn = false;

                }
               
            },
            (err: any) => { this.displayError(err) },
            () => this.alertService.fnLoading(false)
        );
    };
    showStatusBtn: boolean;
    showRejectedBtn: boolean;
    showApprovedBtn: boolean;




    compareDate(pDate) {
        let pd = new Date(Date.parse(pDate));
        var planDate = pd.getFullYear() + "-" + (pd.getMonth() + 1) + "-" + pd.getDate() + " " + 0 + ":" + 0 + ":" + 0;
        var d = new Date();
        var currentDate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate() + " " + 0 + ":" + 0 + ":" + 0;
        var planDateInMileSeconds = Date.parse(planDate);
        var currentDateInMileScondes = Date.parse(currentDate);
        if (planDateInMileSeconds >= currentDateInMileScondes) return true;
        else return false
    }


    back() {
        this.router.navigate(["/journey-plan/line-manager"]);
    }

    onStatusChange(mySelect, jPlan) {

        // debugger;
        this.journeyPlanStatus.planId = jPlan.id;
        this.journeyPlanStatus.status = Number(mySelect);
        let message=this.journeyPlanStatus.status==PlanStatus.ChangeRequested?"Are you sure to request to change the journey plan?":"Are you sure to approve the journey plan?";
      
        if (this.journeyPlanStatus.status == PlanStatus.ChangeRequested) {
            if (!this.journeyPlanStatus.comment) {
                this.alertService.alert("Please leave a comment.");
                return;
            }
        }



        this.alertService.confirm(message, () => {

            //if (PlanStatus.Rejected == Number(mySelect)) {
            //    alert("Rejected");
            //}
            //else if (PlanStatus.Approved == Number(mySelect)) {
            //    alert("Approved");
            //}
            //else return;

            this.alertService.fnLoading(true);
            this.journeyPlanService.ChangePlanStatus(this.journeyPlanStatus).subscribe(
                (res) => {
                
                    this.router.navigate(["/journey-plan/line-manager"]).then(
                        () => {
                            this.alertService.tosterSuccess(`Status changed successfully.`);
                        });
                  
                },
                (error) => {
                    console.log(error);
                    this.displayError(error);
                }

            ).add(() => this.alertService.fnLoading(false));
        }, () => {

        });
    }
    private displayError(errorDetails: any) {

        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }
}
export enum PlanStatusNotEdited {
    Approved = 1,
    ChangeRequested=3
   
}