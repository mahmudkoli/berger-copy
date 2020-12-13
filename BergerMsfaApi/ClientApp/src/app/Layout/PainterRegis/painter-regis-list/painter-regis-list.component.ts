import { Component, OnInit, Inject } from '@angular/core';
import { Painter } from '../../../Shared/Entity/Painter/Painter';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';

import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { PainterRegisService } from '../../../Shared/Services/Painter-Regis/painterRegister.service';

@Component({
    selector: 'app-painter-regis-list',
    templateUrl: './painter-regis-list.component.html',
    styleUrls: ['./painter-regis-list.component.css']
})
export class PainterRegisListComponent implements OnInit {
    permissionGroup: PermissionGroup = new PermissionGroup();
    public painterList: Painter[] = [];
    public baseUrl: string;
    first = 0;

    rows = 5;
    constructor(
        private router: Router,
        private activityPermissionService: ActivityPermissionService,
        private painterRegisSvc: PainterRegisService,
        private alertService: AlertService,
        @Inject('BASE_URL') baseUrl: string) { this.fnPainterList(); this.baseUrl = baseUrl; }

    ngOnInit() {

        this._initPermissionGroup();
       
    }
    detail(id) {

        this.router.navigate(['/painter/detail/' + id]);
    }
    private fnPainterList() {
        this.alertService.fnLoading(true);

        this.painterRegisSvc.GetPainterList().subscribe(
            (res) => {
                this.painterList = res.data || [];;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }
    private _initPermissionGroup() {

        //this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        console.log(this.permissionGroup);
        //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;

        this.ptableSettings.enabledRecordCreateBtn = true;
        this.ptableSettings.enabledEditBtn = true;
        this.ptableSettings.enabledDeleteBtn = true;


    }
    public ptableSettings = {
        tableID: "Painter-List",
        tableClass: "table table-border ",
        tableName: 'Painter List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Name ', width: '10%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Address', width: '30%', internalName: 'address', sort: true, type: "" },
            { headerName: 'Phone', width: '30%', internalName: 'phone', sort: true, type: "" },
            { headerName: 'DepotName', width: '20%', internalName: 'depotName', sort: true, type: "" },
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledDeleteBtn: true,
        enabledEditBtn: true,
        // enabledCellClick: true,
        enabledColumnFilter: true,
        // enabledDataLength:true,
        // enabledColumnResize:true,
        // enabledReflow:true,
        // enabledPdfDownload:true,
        // enabledExcelDownload:true,
        // enabledPrint:true,
        // enabledColumnSetting:true,
        enabledRecordCreateBtn: true,
        // enabledTotal:true,

    };
}
