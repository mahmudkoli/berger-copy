
import { NgbCalendar, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
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
                this.journeyPlan = result.data;
            },
            (err: any) => console.log(err),
            () => this.alertService.fnLoading(false)
        );
    };
    back() {
        this.router.navigate(["/journey-plan/line-manager"]);
    }
    onStatusChange(mySelect, jPlan) {

        debugger;
        if (mySelect.value=="-1") return;
        this.journeyPlanStatus.planId = jPlan.id;
        this.journeyPlanStatus.status = Number(mySelect.value);
         

        this.alertService.confirm(`are you sure to change status?`, () => {
            this.alertService.fnLoading(true);
            this.journeyPlanService.ChangePlanStatus(this.journeyPlanStatus).subscribe(
                (res) => {
                    
                    //  this.fnjourneyplanlist();
                    // this.fnjourneyplanlistpaging(this.first, this.rows, this.pladate);
                    this.router.navigate(["/journey-plan/line-manager"]).then(
                        () => {
                            this.alertService.tosterSuccess(`status successfully.`);
                        });
                  
                },
                (error) => {
                    console.log(error);
                }

            ).add(() => this.alertService.fnLoading(false));
        }, () => {

        });
    }
}
export enum PlanStatusNotEdited {
    Pending = 0,
    Approved = 1,
   
}