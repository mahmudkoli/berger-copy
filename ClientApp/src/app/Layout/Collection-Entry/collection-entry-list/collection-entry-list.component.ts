import { Component, OnInit } from '@angular/core';
import { CollectionEntryService } from '../../../Shared/Services/Collection/collectionentry.service';
import { ActivityPermissionService, PermissionGroup } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';


export enum CustomerType {
    Dealer = 1,
    Subdealer = 2,
}
@Component({
    selector: 'app-collection-entry-list',
    templateUrl: './collection-entry-list.component.html',
    styleUrls: ['./collection-entry-list.component.css']
})

export class CollectionEntryListComponent implements OnInit {


    permissionGroup: PermissionGroup = new PermissionGroup();
    customerTypeList: { key: number, type: string }[] = [

        { key: 1, type: 'Dealer' },
        { key: 2, type: 'Sub Dealer' },
        { key: 3, type: 'Project' },

    ]
    ptableSettings: any = null;
    gridDataSource: any[] = [];
    selectedType: number;
    tableColDef: any;
    constructor(
        private collectionEntryService: CollectionEntryService,
        private alertService: AlertService,
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router) { this._initPermissionGroup(); }

    ngOnInit() {
        this.selectedType = CustomerType.Subdealer;
    }
    private _initPermissionGroup() {

        //this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        //console.log(this.permissionGroup);
        //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;

       

    }
    onChange(val: number) {
        let tableColDef: any;
        let tableName: string = "";

        if (CustomerType.Dealer == val) {
            tableName = "Dealer Collection";
            tableColDef = [
                { headerName: 'Name ', width: '10%', internalName: 'name', sort: true, type: "" },
                { headerName: 'Code  ', width: '30%', internalName: 'code', sort: true, type: "" },
                { headerName: 'Bank Name ', width: '20%', internalName: 'bankName', sort: true, type: "" },


            ];

            this.collectionEntryService.getCollectionByType("Dealer").subscribe(res => {
                this.gridDataSource = res.data;
            })


        }
        else if (CustomerType.Subdealer == val) {
            tableName = "Sub Dealer Collection";
            tableColDef = [
                { headerName: 'Name ', width: '10%', internalName: 'name', sort: true, type: "" },
                { headerName: 'Code  ', width: '30%', internalName: 'Code', sort: true, type: "" },
                { headerName: 'Bank Name ', width: '20%', internalName: 'bankName', sort: true, type: "" },


            ];
            this.collectionEntryService.getCollectionByType("SubDealer").subscribe(res => {
                this.gridDataSource = res.data;
            })
            this.gridDataSource = [];
        }


        this.configureTable(tableName, tableColDef);
    }
    private configureTable(tableName: string, tableCol: any) {
        this.ptableSettings = {
            tableID: "Setup-table",
            tableClass: "table table-border ",
            tableName: tableName,
            tableRowIDInternalName: "Id",
            //tableColDef: [
            //    { headerName: 'Code ', width: '10%', internalName: 'typeCode', sort: true, type: "" },
            //    { headerName: 'Type Name  ', width: '30%', internalName: 'typeName', sort: true, type: "" },
            //    { headerName: 'Dropdown Name ', width: '20%', internalName: 'dropdownName', sort: true, type: "" },
            //    { headerName: 'Sequence', width: '10%', internalName: 'sequence', sort: true, type: "" },

            //],
            tableColDef: tableCol,
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
}
