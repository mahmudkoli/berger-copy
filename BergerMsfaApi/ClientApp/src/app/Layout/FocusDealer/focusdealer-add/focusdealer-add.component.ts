import { Component, OnInit } from '@angular/core';
import { FocusDealer } from '../../../Shared/Entity/FocusDealer/JourneyPlan';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { FocusdealerService } from '../../../Shared/Services/FocusDealer/focusdealer.service';
import { CommonService } from '../../../Shared/Services/Common/common.service';

@Component({
    selector: 'app-focusdealer-add',
    templateUrl: './focusdealer-add.component.html',
    styleUrls: ['./focusdealer-add.component.css']
})
export class FocusdealerAddComponent implements OnInit {

    focusDealerModel: FocusDealer = new FocusDealer();
    dealerList: any[] = [];
    employeeList: any[] = [];

    constructor(
        private alertService: AlertService,
        public formatter: NgbDateParserFormatter,
        private route: ActivatedRoute,
        private focusDealerService: FocusdealerService,
        private commonSvc: CommonService,
        private router: Router
    ) { }


    ngOnInit() {
        this.getEmpList();
        this.getDealerList();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getFocusDealerById(id);
        }

    }
    private get _loggedUser() { return this.commonSvc.getUserInfoFromLocalStorage(); }

    private getEmpList() {
        this.commonSvc.getUserInfoList().subscribe(
            (result: any) => {
                this.employeeList = result.data;
            },
            (err: any) => console.log(err)
        );
    }


    public fnRouteList() {
        this.router.navigate(['/dealer/focusdealer-list'])
    }

    public fnSave() {
        debugger;
        this.focusDealerModel.validFrom = this.focusDealerModel.validFromNgbDate.year.toString() + "-"
            + this.focusDealerModel.validFromNgbDate.month.toString() + "-"
            + this.focusDealerModel.validFromNgbDate.day.toString();


        this.focusDealerModel.validTo = this.focusDealerModel.validToNgbDate.year.toString() + "-"
            + this.focusDealerModel.validToNgbDate.month.toString() + "-"
            + this.focusDealerModel.validToNgbDate.day.toString();


        this.focusDealerModel.id == 0 ? this.insert(this.focusDealerModel) : this.update(this.focusDealerModel);
    }

    private insert(focusDealer: FocusDealer) {

        this.focusDealerService.create(focusDealer).subscribe(res => {
            console.log("focus-dealer response: ", res);
            this.router.navigate(['/dealer/focusdealer-list']).then(() => {
                this.alertService.tosterSuccess("focus-dealer has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }
    private update(model: FocusDealer) {
        this.focusDealerService.update(model).subscribe(res => {
            console.log("focus update res: ", res);
            this.router.navigate(['/dealer/focusdealer-list']).then(() => {
                this.alertService.tosterSuccess("focus-dealer has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private getFocusDealerById(id) {
        this.focusDealerService.getFocusDealerById(id).subscribe(
            (result: any) => {
                let focusDealer = result.data as FocusDealer;
                focusDealer.validFromNgbDate = NgbDate.from(this.formatter.parse(focusDealer.validFrom));
                focusDealer.validToNgbDate = NgbDate.from(this.formatter.parse(focusDealer.validTo));
                this.editForm(focusDealer);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(focusDealer: FocusDealer) {
        this.focusDealerModel = focusDealer;
    }

    private getDealerList() {
        if (this._loggedUser) {
            this.alertService.fnLoading(true);
            this.commonSvc.getDealerList(this._loggedUser.userCategory, this._loggedUser.userCategoryIds).subscribe(
                (result: any) => {
                    this.dealerList = result.data;
                },
                (err: any) => console.log(err)

            ).add(() => this.alertService.fnLoading(false));
        }

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
