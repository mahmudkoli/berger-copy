import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmailConfigForDealerSalesCall } from 'src/app/Shared/Entity/DealerSalesCall/EmailConfigForDealerSalesCall';
import { Dropdown } from 'src/app/Shared/Entity/Setup/dropdown';
import { EnumDealerSalesCallIssueCategory, EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { EnumEmployeeRoleLabel } from 'src/app/Shared/Enums/employee-role';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import { FocusDealerService } from 'src/app/Shared/Services/FocusDealer/focus-dealer.service';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { EmailConfigForDealerOpening } from '../../../Shared/Entity/DealerOpening/EmailConfig';

@Component({
  selector: 'app-email-config-add',
  templateUrl: './dealer-sales-call-email-config-add.component.html',
  styleUrls: ['./dealer-sales-call-email-config-add.component.css']
})
export class DealerSalesCallEmailConfigAddComponent implements OnInit {

  emailConfigModel: EmailConfigForDealerSalesCall = new EmailConfigForDealerSalesCall();
    dealerList: any[] = [];
    employeeList: any[] = [];
	categories: Dropdown[] = [];


    constructor(
        private alertService: AlertService,
        public formatter: NgbDateParserFormatter,
        private route: ActivatedRoute,
        private focusDealerService: DealerSalesCallService,
        private dynamicDropdownService: DynamicDropdownService,
        private commonSvc: CommonService,
        private router: Router
    ) { }


    ngOnInit() {
        this.loadCategories();

        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getEmailConfigById(id);
        }
    }
    private get _loggedUser() { return this.commonSvc.getUserInfoFromLocalStorage(); }

    private getEmpList() {
        this.commonSvc.getUserInfoListByCurrentUser().subscribe(
            (result: any) => {
                this.employeeList = result.data;
            },
            (err: any) => console.log(err)
        );
    }
    loadCategories() {
		this.alertService.fnLoading(true);
		const categoryCode = EnumDynamicTypeCode.ISSUES_01;
		this.dynamicDropdownService.GetDropdownByTypeCd(categoryCode)
			.subscribe(res => {
				this.categories = res.data;
            });
        }

    public fnRouteList() {
        this.router.navigate(['/dealer-sales-call/emailList'])
    }

    public fnSave() {
        // debugger;
        


        this.emailConfigModel.id == 0 ? this.insert(this.emailConfigModel) : this.update(this.emailConfigModel);
    }

    private insert(emailConfigModel: EmailConfigForDealerSalesCall) {

      console.log("Email Config",this.emailConfigModel)
        this.focusDealerService.createEmailConfig(emailConfigModel).subscribe(res => {
            console.log("focus-dealer response: ", res);
            this.router.navigate(['/dealer-sales-call/emailList']).then(() => {
                this.alertService.tosterSuccess("Email Configuration has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }
    private update(model: EmailConfigForDealerSalesCall) {
        this.focusDealerService.updateEmailConfig(model).subscribe(res => {
            console.log("focus update res: ", res);
            this.router.navigate(['/dealer-sales-call/emailList']).then(() => {
                this.alertService.tosterSuccess("Email Configuration has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private getEmailConfigById(id) {
        this.focusDealerService.getEmailConfigById(id).subscribe(
            (result: any) => {
                let emailconfig = result.data as EmailConfigForDealerSalesCall;
                this.editForm(emailconfig);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(emailconfig: EmailConfigForDealerSalesCall) {
        this.emailConfigModel = emailconfig;
    }

    // private getDealerList() {
    //     if (this._loggedUser) {
    //         this.alertService.fnLoading(true);
    //         this.commonSvc.getDealerList(this._loggedUser.userCategory, this._loggedUser.userCategoryIds).subscribe(
    //             (result: any) => {
    //                 this.dealerList = result.data;
    //             },
    //             (err: any) => console.log(err)

    //         ).add(() => this.alertService.fnLoading(false));
    //     }

    // }
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
