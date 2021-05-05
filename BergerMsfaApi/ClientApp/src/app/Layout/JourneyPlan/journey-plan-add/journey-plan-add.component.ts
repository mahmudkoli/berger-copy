import { Component, OnInit } from '@angular/core';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { NgbDateParserFormatter, NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from '../../../Shared/Services/Common/common.service';
import { of } from 'rxjs';
import { delay, take } from 'rxjs/operators';

@Component({
    selector: 'app-journey-plan-add',
    templateUrl: './journey-plan-add.component.html',
    styleUrls: ['./journey-plan-add.component.css']
})


export class JourneyPlanAddComponent implements OnInit {

    journeyPlanModel: JourneyPlan = new JourneyPlan();
    dealerList: any[] = [];
    dealerSelection: any;
    dealers: any[] = [];
    subDealers: any[] = [];
    readonly: boolean = false;
    dealerClass: any;
    subDealerClass: any;
    constructor(
        private commonSvc: CommonService,
        private calender: NgbCalendar,
        public formatter: NgbDateParserFormatter,
        private alertService: AlertService,
        private route: ActivatedRoute,
        private journeyPlanService: JourneyPlanService,
        private router: Router
    ) { }

    private get _loggedUser() { return this.commonSvc.getUserInfoFromLocalStorage(); }
    ngOnInit() {
        
        this.dealerClass = "btn btn-primary active";
        this.subDealerClass = "btn btn-secondary";
        this.getDealerList();
        // debugger;
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);


        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.date !== 'undefined') {
            console.log("id", this.route.snapshot.params.date);
            let date = this.route.snapshot.params.date;
            this.getJourneyPlanById(date);
        }

        else
            this.journeyPlanModel.visitDDate = this.calender.getToday();

    }
    private getDealerList() {

        if (this._loggedUser) {
            this.alertService.fnLoading(true);
            this.commonSvc.getDealerList(this._loggedUser.userCategory, this._loggedUser.userCategoryIds).subscribe(
                (result: any) => {
                    this.dealerList = result.data;
                    
                    this.dealerList.forEach(p => {
                        // p.customerName = p.customerName + '-' + p.customerNo;
                        (p.isSubdealer) ? this.subDealers.push(p) : this.dealers.push(p);

                    });
                    this.dealerList = this.dealers;
                    //console.log(this.dealerList);
                    /*console.log("dealers: " + this.dealers.length + " -- subdealser: " + this.subDealers.length);*/
                   
                },
                (err: any) => console.log(err)

            ).add(() => this.alertService.fnLoading(false));
        }


    }
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

    delete(jPlan) {
        // debugger;
        this.journeyPlanModel.visitDate = this.journeyPlanModel.visitDDate.year.toString() + "-" + this.journeyPlanModel.visitDDate.month.toString() + "-" + this.journeyPlanModel.visitDDate.day.toString();
        if (this.compareDate(this.journeyPlanModel.visitDate)) {
            this.alertService.confirm("Are you sure you want to delete this item?", () => {
                this.alertService.fnLoading(true);
                this.journeyPlanService.delete(this.journeyPlanModel.id).subscribe(
                    (res: any) => {
                        console.log('res from del func', res);
                        this.alertService.tosterSuccess("Journey plan has been deleted successfully.");

                        // this.onLoadJourneyPlans(this.first, this.rows, this.search);
                    },
                    (error) => {
                        console.log(error);
                        this.displayError(error);
                    }
                ).add(() => this.alertService.fnLoading(false));;
            }, () => {

            });
        }
        else this.alertService.alert("Can not delete pervious plan.");
    }
    private getJourneyPlanById(date) {
        this.alertService.fnLoading(true);
        this.journeyPlanService.getJourneyPlanById(date).subscribe(
            (result: any) => {

                this.readonly=true;
                let editData = result.data as JourneyPlan;
                if(editData==null) {
                    this.journeyPlanModel.visitDDate=NgbDate.from(this.formatter.parse(date));;
                    return;
                }
                editData.visitDDate = NgbDate.from(this.formatter.parse(editData.visitDate));

                this.editForm(editData);
            },
            (err: any) => console.log(err),
            () => this.alertService.fnLoading(false)
        );
    };

    public fnRouteList() {
        this.router.navigate(['/journey-plan/list']);
    }

    public fnSave() {
        this.journeyPlanModel.visitDate = this.journeyPlanModel.visitDDate.year.toString() + "-" + this.journeyPlanModel.visitDDate.month.toString() + "-" + this.journeyPlanModel.visitDDate.day.toString();
        this.journeyPlanModel.id == 0 ? this.insert(this.journeyPlanModel) : this.update(this.journeyPlanModel);
    }

    private insert(journeyPlan: JourneyPlan) {
        this.alertService.fnLoading(true);
        this.journeyPlanService.create(journeyPlan).subscribe(
            (res) => {
                console.log("JourneyPlan res: ", res);
                this.router.navigate(['/journey-plan/list']).then(
                    () => {
                        this.alertService.tosterSuccess("JourneyPlan has been created successfully.");
                    });
            },
            (error) => {
                console.log(error);
                this.displayError(error);
            },
        ).add(() => this.alertService.fnLoading(false));
    }

    private update(model: JourneyPlan) {
        this.alertService.fnLoading(true);
        this.journeyPlanService.update(model).subscribe(res => {
            console.log("Journey update res: ", res);
            this.router.navigate(['/journey-plan/list']).then(() => {
                this.alertService.tosterSuccess("Journey-plan has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }
        ).add(() => this.alertService.fnLoading(false));;
    }

    private editForm(journeyPlan: JourneyPlan) {
        this.journeyPlanModel = journeyPlan;
        
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

    public displayDealersData() {
        this.alertService.fnLoading(true);
        of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
            
            this.dealerClass = "btn btn-primary";
            this.subDealerClass = "btn btn-secondary";
            this.dealerList = this.dealers;
            
        }).add(() => this.alertService.fnLoading(false)); 

    }
    public displaySubDealersData() {
        this.alertService.fnLoading(true);
        of(undefined).pipe(take(1), delay(1000)).subscribe(() => {

            this.dealerClass = "btn btn-secondary";
            this.subDealerClass = "btn btn-primary";

            this.dealerList = this.subDealers;

        }).add(() => this.alertService.fnLoading(false)); 
        
    }
}
