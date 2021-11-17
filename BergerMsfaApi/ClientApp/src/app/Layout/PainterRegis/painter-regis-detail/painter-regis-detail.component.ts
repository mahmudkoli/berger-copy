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
import { AuthService } from 'src/app/Shared/Services/Users';
import { finalize } from 'rxjs/operators';
@Component({
    selector: 'app-painter-regis-detail',
    templateUrl: './painter-regis-detail.component.html',
    styleUrls: ['./painter-regis-detail.component.css']
})
export class PainterRegisDetailComponent implements OnInit {
    public baseUrl: string;
    painter: any;
    attachedDealers: any[];
    painterCalls: PainterCall[];
    attachments: any[];

    //dummy data
    //image: any = "uploads//images//Painters//BirtCertificateNo.jpg";
    //attachments = [
    //    {
    //        path: "/abc",
    //        name: "passport",
    //    },
    //    {
    //        path: "/abc",
    //        name: "nid",
    //    },
    //    {
    //        path: "/abc",
    //        name: "birth",
    //    }
    //];
    constructor(

        private alertService: AlertService,
        private route: ActivatedRoute,
        private painterRegisSvc: PainterRegisService,
        private router: Router,
        private modalService: NgbModal,
        private authService: AuthService,
        @Inject('BASE_URL') baseUrl: string) { this.baseUrl = baseUrl; }

    ngOnInit() {
        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this._getRegisterPainterById(id);
        }
        
		this.ptableSettingsPainterCall.enabledDeleteBtn = this.authService.isAdmin;
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
                    obj.isAppInstalledText = obj.isAppInstalled ? "Yes" : "No";
                    obj.hasDbblText = obj.hasDbbl ? "Yes" : "No";

                    obj.viewDetailsText = 'View Details';
                    obj.viewDetailsBtnclass = 'btn-transition btn btn-sm btn-outline-primary d-flex align-items-center';
                    

                });


            },
            (err: any) => console.log(err)
        );
        
    }
   
    back() {
        this.router.navigate(['painter/register-list']);
    }


    public ptableSettingsPainterCall: IPTableSetting = {
        tableID: "painterCall-table",
        tableClass: "table table-border ",
        tableName: 'PainterCall List',
        tableRowIDInternalName: "id",
        tableColDef: [
            { headerName: 'Territory', width: '10%', internalName: 'territory', sort: false, type: "" },
            { headerName: 'Zone', width: '10%', internalName: 'zone', sort: false, type: "" },
            { headerName: 'Create Time', width: '10%', internalName: 'createdTimeStr', sort: false, type: "" },
            { headerName: 'Painter Category', width: '10%', internalName: 'painterCatName', sort: false, type: "" },
            { headerName: 'Painter Name', width: '10%', internalName: 'painterName', sort: false, type: "" },
            { headerName: 'Address', width: '10%', internalName: 'address', sort: false, type: "" },
            { headerName: 'Phone', width: '10%', internalName: 'phone', sort: false, type: "" },
            { headerName: 'Is App Installed', width: '10%', internalName: 'isAppInstalledText', sort: false, type: "" },
            { headerName: 'Has Dbbl', width: '10%', internalName: 'hasDbblText', sort: false, type: "" },
            { headerName: 'Acc Dbbl Number', width: '10%', internalName: 'accDbblNumber', sort: false, type: "" },
            { headerName: 'Acc Dbbl Holder Name', width: '10%', internalName: 'accDbblHolderName', sort: false, type: "" },
            { headerName: 'Acc Change Reason', width: '10%', internalName: 'accChangeReason', sort: false, type: "" },
            { headerName: 'Has Dbbl Issue', width: '10%', internalName: 'hasDbblIssueText', sort: false, type: "" },
            { headerName: 'Comment', width: '10%', internalName: 'comment', sort: false, type: "" },
            { headerName: 'Details', width: '5%', internalName: 'viewDetailsText', sort: false, type: "button", onClick: 'true', className:'viewDetailsBtnclass', innerBtnIcon:''}



        ],
        //enabledSearch: true,
        enabledSerialNo: true,
        // pageSize: 10,
        enabledPagination: true,
        // enabledDeleteBtn: true,
        // enabledEditBtn: true,
        enabledCellClick: true,
        //enabledColumnFilter: false,
        // enabledRecordCreateBtn: true,
        enabledDataLength: true,
        // newRecordButtonText: 'New ELearning'
    };

    public ptableSettingsAttachedDealers: IPTableSetting = {
        tableID: "attachedDealers-table",
        tableClass: "table table-border ",
        tableName: 'Attached Dealers List',
        tableRowIDInternalName: "id",
        tableColDef: [
            { headerName: 'Dealer Name', width: '50%', internalName: 'customerName', sort: false, type: "" },
            { headerName: 'Dealer No', width: '50%', internalName: 'customerNo', sort: false, type: "" },
            

        ],
        //enabledSearch: true,
        enabledSerialNo: true,
        // pageSize: 10,
        enabledPagination: true,
        // enabledDeleteBtn: true,
        // enabledEditBtn: true,
        //enabledCellClick: true,
        //enabledColumnFilter: false,
        // enabledRecordCreateBtn: true,
        enabledDataLength: true,
        // newRecordButtonText: 'New ELearning'
    };

    
    public cellClickCallbackFn(event: any) {
        let cellName = event.cellName;
        let companyMTDData = event.record.painterCompanyMTDValue;
        let dealerData = event.record.attachedDealers;
        if (cellName == 'viewDetailsText') {
            this.detailsPainterCall(companyMTDData, dealerData);
        }
    }
    detailsPainterCall(companyMTDData, dealerData) {
        console.log(companyMTDData);
        console.log(dealerData);
        this.openDetailsPainterCallModal(companyMTDData, dealerData);
    }
    openDetailsPainterCallModal(companyMTDData: any, dealerData: any) {
        let ngbModalOptions: NgbModalOptions = {
            backdrop: "static",
            keyboard: false,
            size: "lg",
        };
        const modalRef = this.modalService.open(
            ModalPainterCallDetailsComponent,
            ngbModalOptions
        );
        modalRef.componentInstance.painterCompanyMTDValue = companyMTDData;
        modalRef.componentInstance.attachedDealers = dealerData;

        modalRef.result.then(
            (result) => {
                console.log(result);
            },
            (reason) => {
                console.log(reason);
            }
        );
    }

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		
		if (event.action == "delete-item") {
			this.deletePainterCallUp(event.record.id);
			
		}
	}

	deletePainterCallUp(id) {
		this.alertService.confirm("Are you sure to delete this painter call?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.painterRegisSvc.DeletePainterCall(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Painter call has been deleted successfully.");
							this.painterCalls.forEach((value, index) => {
								if (value.id == id) this.painterCalls.splice(index, 1);
							});
						},
						(error) => {
							console.log(error);
						});
				// this.subscriptions.push(deleteSubscription);
			},
			() => { });
	}

}
