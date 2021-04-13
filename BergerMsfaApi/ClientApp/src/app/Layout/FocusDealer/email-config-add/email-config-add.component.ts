import { Component, OnInit } from '@angular/core';
import { ControlContainer } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { FocusDealer } from 'src/app/Shared/Entity/FocusDealer/JourneyPlan';
import { EnumEmployeeRoleLabel } from 'src/app/Shared/Enums/employee-role';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { FocusdealerService } from 'src/app/Shared/Services/FocusDealer/focusdealer.service';
import { EmailConfigForDealerOpening } from '../../../Shared/Entity/DealerOpening/EmailConfig';

@Component({
  selector: 'app-email-config-add',
  templateUrl: './email-config-add.component.html',
  styleUrls: ['./email-config-add.component.css']
})
export class EmailConfigAddComponent implements OnInit {

  emailConfigModel: EmailConfigForDealerOpening = new EmailConfigForDealerOpening();
    dealerList: any[] = [];
    employeeList: any[] = [];
    employeeRoles: MapObject[] = EnumEmployeeRoleLabel.EmployeeRoles;
    plants: any[] = [];

    constructor(
        private alertService: AlertService,
        public formatter: NgbDateParserFormatter,
        private route: ActivatedRoute,
        private focusDealerService: FocusdealerService,
        private commonSvc: CommonService,
        private router: Router
    ) { }


    ngOnInit() {

        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getEmailConfigById(id);
            
        }

        
        this.commonSvc.getDepotList().subscribe((p) => {
            this.plants = p.data;
            
            
        }), (err: any) => console.log(err);

    }
    private get _loggedUser() { return this.commonSvc.getUserInfoFromLocalStorage(); }

    private getEmpList() {
        this.commonSvc.getUserInfoListByLoggedInManager().subscribe(
            (result: any) => {
                this.employeeList = result.data;
                console.log(this.employeeList);
            },
            (err: any) => console.log(err)
        );
    }


    public fnRouteList() {
        this.router.navigate(['/dealer/email'])
    }

    public fnSave() {
        // debugger;

       // console.log(this.emailConfigModel);
        
        this.emailConfigModel.id == 0 ? this.insert(this.emailConfigModel) : this.update(this.emailConfigModel);
    }

    private insert(emailConfigModel: EmailConfigForDealerOpening) {

      console.log("Email Config",this.emailConfigModel)
        this.focusDealerService.createEmailConfig(emailConfigModel).subscribe(res => {
            console.log("focus-dealer response: ", res);
            this.router.navigate(['/dealer/email']).then(() => {
                this.alertService.tosterSuccess("Email Configuration has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }
    private update(model: EmailConfigForDealerOpening) {
        this.focusDealerService.updateEmailConfig(model).subscribe(res => {
            console.log("focus update res: ", res);
            this.router.navigate(['/dealer/email']).then(() => {
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
                let emailconfig = result.data as EmailConfigForDealerOpening;
                console.log(emailconfig);
                this.editForm(emailconfig);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(emailconfig: EmailConfigForDealerOpening) {
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
