import { Component, OnInit, Inject } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';

import { Painter } from '../../../Shared/Entity/Painter/Painter';
import { PainterRegisService } from '../../../Shared/Services/Painter-Regis/painterRegister.service';
import { Observable } from 'rxjs';
import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { PainterCall } from '../../../Shared/Entity/Painter/PainterCall';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ModalPainterCallDetailsComponent } from '../modal-painter-call-details/modal-painter-call-details.component'
@Component({
    selector: 'app-painter-update-status',
    templateUrl: './painter-update-status.component.html',
    styleUrls: ['./painter-update-status.component.css']
})
export class PainterUpdateStatusComponent implements OnInit {
    public baseUrl: string;
    painter: any;
    attachedDealers: any[];
    painterCalls: PainterCall[];
    attachments: any[];
    statusType: any[] = [];
    required: boolean = true;

    constructor(
        private alertService: AlertService,
        private route: ActivatedRoute,
        private painterRegisSvc: PainterRegisService,
        private router: Router,
        private modalService: NgbModal,
        @Inject('BASE_URL') baseUrl: string) { this.baseUrl = baseUrl; }

    ngOnInit() {
        this.statusType = this.getStatusType();
        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this._getRegisterPainterById(id);
        }
    }

    private getStatusType() : any[] {
        const statusType = [{'id': 0, 'name': 'Inactive'}, {'id': 1, 'name': 'Active'}]
        return statusType;
    }

    private _getRegisterPainterById(id) {
        this.painterRegisSvc.GetRegisterPainterById(id).subscribe(
            (result: any) => {
                this.painter = result.data;
                this.painterCalls = this.painter.painterCalls;
                this.attachedDealers = this.painter.dealerDetails;
                this.attachments = this.painter.attachments;

                console.log(this.painter);
                this.painterCalls.forEach(obj => {
                    obj.hasAppUsageText = obj.hasAppUsage ? "Yes" : "No";
                    obj.hasDbblIssueText = obj.hasDbblIssue ? "Yes" : "No";
                    obj.hasNewProBriefingText = obj.hasNewProBriefing ? "Yes" : "No";
                    obj.hasUsageEftToolsText = obj.hasUsageEftTools ? "Yes" : "No";
                    obj.hasPremiumProtBriefingText = obj.hasPremiumProtBriefing ? "Yes" : "No";
                    obj.hasSchemeComnunactionText = obj.hasSchemeComnunaction ? "Yes" : "No";

                    obj.viewDetailsText = 'View Details';
                    obj.viewDetailsBtnclass = 'btn-transition btn btn-sm btn-outline-primary d-flex align-items-center';
                });
            },
            (err: any) => console.log(err)
        );
    }
    
    public fnSave() {
        this.update(this.painter);
    }

    private update(model: any) {
        this.painterRegisSvc.UpdatePainterStatus(model).subscribe(res => {
            console.log("Painter update res: ", res);
            this.router.navigate(['/painter/register-list']).then(() => {
                this.alertService.tosterSuccess("Painter status updated successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
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

    back() {
        this.router.navigate(['painter/register-list']);
    }

}
