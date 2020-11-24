import { Component, OnInit } from '@angular/core';
import { NgbCalendar, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { PlanStatus } from '../../../Shared/Enums/PlanStatus';

@Component({
    selector: 'app-journey-plan-detail',
    templateUrl: './journey-plan-detail.component.html',
    styleUrls: ['./journey-plan-detail.component.css']
})
export class JourneyPlanDetailComponent implements OnInit {
    PlanStatusEnum = PlanStatus;
    journeyPlan: any;
    constructor(
        public alertService: AlertService,
        public formatter: NgbDateParserFormatter,
        private route: ActivatedRoute,
        private journeyPlanService: JourneyPlanService,
        private router: Router) { }

    ngOnInit() {
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
            (err: any) => this.displayError(err),
            () => this.alertService.fnLoading(false)
        );
    };
    back() {
        this.router.navigate(["/journey-plan/list"]);
    }
    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
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
