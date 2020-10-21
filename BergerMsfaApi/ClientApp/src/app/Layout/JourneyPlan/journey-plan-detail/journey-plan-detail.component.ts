import { Component, OnInit } from '@angular/core';
import { NgbCalendar, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
    selector: 'app-journey-plan-detail',
    templateUrl: './journey-plan-detail.component.html',
    styleUrls: ['./journey-plan-detail.component.css']
})
export class JourneyPlanDetailComponent implements OnInit {

    journeyPlan: any;
    constructor(
        public alertServic: AlertService,
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
        this.alertServic.fnLoading(true);
        this.journeyPlanService.getJourneyPlanDetailById(id).subscribe(
            (result: any) => {
                this.journeyPlan = result.data;
            },
            (err: any) => console.log(err),
            () => this.alertServic.fnLoading(false)
        );
    };
    back() {
        this.router.navigate(["/journey-plan/list"]);
    }

}
