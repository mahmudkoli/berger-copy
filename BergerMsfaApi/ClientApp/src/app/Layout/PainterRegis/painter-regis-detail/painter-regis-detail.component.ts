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
        @Inject('BASE_URL') baseUrl: string) { this.baseUrl = baseUrl; }

    ngOnInit() {
        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this._getRegisterPainterById(id);
        }
        
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
   
    back() {
        this.router.navigate(['painter/register-list']);
    }


    public ptableSettingsPainterCall: IPTableSetting = {
        tableID: "painterCall-table",
        tableClass: "table table-border ",
        tableName: 'PainterCall List',
        tableRowIDInternalName: "id",
        tableColDef: [
            { headerName: 'Comment', width: '12%', internalName: 'comment', sort: false, type: "" },
            { headerName: 'Work In Hand Number', width: '12%', internalName: 'workInHandNumber', sort: false, type: "" },
            { headerName: 'Has Scheme Comnunaction', width: '12%', internalName: 'hasSchemeComnunactionText', sort: false, type: "" },
            { headerName: 'Has Premium Prot Briefing', width: '12%', internalName: 'hasPremiumProtBriefingText', sort: false, type: "" },
            { headerName: 'Has New Pro Briefing', width: '12%', internalName: 'hasNewProBriefingText', sort: false, type: "" },
            { headerName: 'Has Usage Eft Tools', width: '12%', internalName: 'hasUsageEftToolsText', sort: false, type: "" },
            { headerName: 'Has App Usage', width: '12%', internalName: 'hasAppUsageText', sort: false, type: "" },
            { headerName: 'Has Dbbl Issue', width: '12%', internalName: 'hasDbblIssueText', sort: false, type: "" },
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
        let data = event.record.painterCompanyMTDValue;
        if (cellName == 'viewDetailsText') {
            this.detailsPainterCall(data);
        }
    }
    detailsPainterCall(data) {
        console.log(data);
        this.openDetailsPainterCallModal(data);
    }
    openDetailsPainterCallModal(data: any) {
        let ngbModalOptions: NgbModalOptions = {
            backdrop: "static",
            keyboard: false,
            size: "lg",
        };
        const modalRef = this.modalService.open(
            ModalPainterCallDetailsComponent,
            ngbModalOptions
        );
        modalRef.componentInstance.painterCompanyMTDValue = data;

        modalRef.result.then(
            (result) => {
                console.log(result);
            },
            (reason) => {
                console.log(reason);
            }
        );
    }

}
