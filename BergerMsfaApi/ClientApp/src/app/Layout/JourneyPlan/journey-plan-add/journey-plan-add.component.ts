import { Component, OnInit } from '@angular/core';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { NgbDateParserFormatter, NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-journey-plan-add',
  templateUrl: './journey-plan-add.component.html',
  styleUrls: ['./journey-plan-add.component.css']
})


export class JourneyPlanAddComponent implements OnInit {

    journeyPlanModel: JourneyPlan = new JourneyPlan();
    dealerList: any[] = [];
    employeeRegList: any[] = [];
    date: { year: number, month: number };

    constructor(
        private calender: NgbCalendar,
        public formatter: NgbDateParserFormatter,
        private alertService: AlertService,
        private route: ActivatedRoute,
        private dynamicDropdownService: DynamicDropdownService,
        private journeyPlanService: JourneyPlanService,
        private router: Router
    ) { }

    ngOnInit() {

        this.getEmpList();

        this.getDealerList();

        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getJourneyPlanById(id);
        }

        else
            this.journeyPlanModel.visitDDate = this.calender.getToday();

  }
    private getEmpList() {
        //hard code param for temporary
        this.dynamicDropdownService.GetDropdownByTypeCd("E01").subscribe(
            (result: any) => {
                this.employeeRegList = result.data;
            },
            (err: any) => console.log(err)
        );
    }

    public fnRouteList() {
        this.router.navigate(['/journey-plan/list']);
    }

    public fnSave() {
        this.journeyPlanModel.visitDate = this.journeyPlanModel.visitDDate.year.toString() + "-" + this.journeyPlanModel.visitDDate.month.toString() + "-" + this.journeyPlanModel.visitDDate.day.toString();
        this.journeyPlanModel.id == 0 ? this.insert(this.journeyPlanModel) : this.update(this.journeyPlanModel);
    }


    private insert(journeyPlan: JourneyPlan) {

        this.journeyPlanService.create(journeyPlan).subscribe(res => {
            console.log("JourneyPlan res: ", res);
            this.router.navigate(['/journey-plan/list']).then(() => {
                this.alertService.tosterSuccess("JourneyPlan has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private update(model: JourneyPlan) {
        this.journeyPlanService.update(model).subscribe(res => {
            console.log("Journey update res: ", res);
            this.router.navigate(['/journey-plan/list']).then(() => {
                this.alertService.tosterSuccess("journey-plan has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }
    private getJourneyPlanById(id) {
        this.journeyPlanService.getJourneyPlanById(id).subscribe(
            (result: any) => {
                let editData = result.data as JourneyPlan;
                editData.visitDDate = NgbDate.from(this.formatter.parse(editData.visitDate));
                this.editForm(editData);
            },
            (err: any) => console.log(err)
        );
    };
    private editForm(journeyPlan: JourneyPlan) {
        this.journeyPlanModel = journeyPlan;
    }

    private getDealerList() {
        //hard code param for temporary
        this.dynamicDropdownService.GetDropdownByTypeCd("D01").subscribe(
            (result: any) => {
                this.dealerList = result.data;
            },
            (err: any) => console.log(err)
        );
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
