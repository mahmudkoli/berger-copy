import { Component, OnInit } from '@angular/core';
import { FocusDealer } from '../../../Shared/Entity/FocusDealer/JourneyPlan';
import { ActivatedRoute, Router } from '@angular/router';
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { FocusdealerService } from '../../../Shared/Services/FocusDealer/focusdealer.service';

@Component({
    selector: 'app-focusdealer-add',
    templateUrl: './focusdealer-add.component.html',
    styleUrls: ['./focusdealer-add.component.css']
})
export class FocusdealerAddComponent implements OnInit {

    focusDealerModel: FocusDealer = new FocusDealer();
    dealerList: any[] = [];
    employeeRegList: any[] = [];

    constructor(
        private alertService: AlertService,
        public formatter: NgbDateParserFormatter,
        private route: ActivatedRoute,
        private dynamicDropdownService: DynamicDropdownService,
        private focusDealerService: FocusdealerService,
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
        this.router.navigate(['/focus-dealer/list']);
    }

    public fnSave() {

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
            this.router.navigate(['/focus-dealer/list']).then(() => {
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
            this.router.navigate(['/focus-dealer/list']).then(() => {
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
        //hard code param for temporary
        this.dynamicDropdownService.getDealerList().subscribe(
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
