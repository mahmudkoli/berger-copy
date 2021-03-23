import { Component, OnInit, Inject } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';

import { Painter } from '../../../Shared/Entity/Painter/Painter';
import { PainterRegisService } from '../../../Shared/Services/Painter-Regis/painterRegister.service';
import { Observable } from 'rxjs';
import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { PainterCall } from '../../../Shared/Entity/Painter/PainterCall';

@Component({
    selector: 'app-painter-regis-detail',
    templateUrl: './painter-regis-detail.component.html',
    styleUrls: ['./painter-regis-detail.component.css']
})
export class PainterRegisDetailComponent implements OnInit {
    public baseUrl: string;
    painter: any;
    image: any = "uploads//images//Painters//BirtCertificateNo.jpg";
    attachedDealers: any;
    painterCallList: PainterCall[];
    viewDetailsBtnclass:string = 'btn-transition btn btn-sm btn-outline-primary d-flex align-items-center';
    attachments = [
        {
            path: "xyz",
            name: "passport",
        },
        {
            path: "abc",
            name: "nid",
        },
        {
            path: "bleh",
            name: "birth",
        }
    ];
    constructor(

        private alertService: AlertService,
        private route: ActivatedRoute,
        private painterRegisSvc: PainterRegisService,
        private router: Router,
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
                this.painterCallList = this.painter.painterCallList;
                console.log(this.painterCallList);
                this.painterCallList.forEach(obj => {
                    obj.viewDetailsText = 'View Log Details';
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
        tableID: "painter call-table",
        tableClass: "table table-border ",
        tableName: 'Painter Call List',
        tableRowIDInternalName: "id",
        tableColDef: [
            { headerName: 'Comment', width: '12%', internalName: 'comment', sort: false, type: "" },
            { headerName: 'Work In Hand Number', width: '12%', internalName: 'workInHandNumber', sort: false, type: "" },
            { headerName: 'Has Scheme Comnunaction', width: '12%', internalName: 'hasSchemeComnunaction', sort: false, type: "" },
            { headerName: 'Has Premium Prot Briefing', width: '12%', internalName: 'hasPremiumProtBriefing', sort: false, type: "" },
            { headerName: 'Has New Pro Briefing', width: '12%', internalName: 'hasNewProBriefing', sort: false, type: "" },
            { headerName: 'Has Usage Eft Tools', width: '12%', internalName: 'hasUsageEftTools', sort: false, type: "" },
            { headerName: 'Has App Usage', width: '12%', internalName: 'hasAppUsage', sort: false, type: "" },
            { headerName: 'Has Dbbl Issue', width: '12%', internalName: 'hasDbblIssue', sort: false, type: "" },
            { headerName: 'Details', width: '5%', internalName: 'viewDetailsText', sort: false, type: "button", onClick: 'true', className:'viewDetailsBtnclass', innerBtnIcon:''}



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

    public cellClickCallbackFn(event) {

    }

}
