import { Component, OnInit } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivityPermissionService, PermissionGroup } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute, Router } from '@angular/router';

import { Dropdown } from '../../../Shared/Entity/Setup/dropdown';
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';

@Component({
    selector: 'app-dropdown-list',
    templateUrl: './dropdown-list.component.html',
    styleUrls: ['./dropdown-list.component.css']
})
export class DropdownListComponent implements OnInit {


    permissionGroup: PermissionGroup = new PermissionGroup();

    public dropdownTypeDetailList: Dropdown[] = [];


    constructor(
        private dnamiynDropdownService: DynamicDropdownService,
        private alertService: AlertService,
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router) {

        this._initPermissionGroup();
    }

    ngOnInit() {
        this.fnDropdownList();
    }

    private _initPermissionGroup() {

        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        console.log(this.permissionGroup);
        //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;

        this.ptableSettings.enabledRecordCreateBtn = true;
        this.ptableSettings.enabledEditBtn = true;
        this.ptableSettings.enabledDeleteBtn = true;


    }

    private fnDropdownList() {
        this.alertService.fnLoading(true);
        this.dnamiynDropdownService.getDropdownList().subscribe(
            (res) => {
                this.dropdownTypeDetailList = res.data || [];;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    public fnPtableCellClick(event) {
        console.log("cell click: ");
    }

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.add();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }

    private delete(id: number) {
        console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.dnamiynDropdownService.delete(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("dropdown has been deleted successfully.");
                    this.fnDropdownList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }


    public ptableSettings = {
        tableID: "Setup-table",
        tableClass: "table table-border ",
        tableName: 'Dynamic Drowdown Setup List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Code ', width: '10%', internalName: 'typeCode', sort: true, type: "" },
            { headerName: 'Type Name  ', width: '30%', internalName: 'typeName', sort: true, type: "" },
            { headerName: 'Dropdown Name ', width: '20%', internalName: 'dropdownName', sort: true, type: "" },
            { headerName: 'Sequence', width: '10%', internalName: 'sequence', sort: true, type: "" },

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
    private add() {
        this.router.navigate(['/setup/dropdown-add']);
    }

    private edit(id: number) {
        console.log('edit product', id);
        this.router.navigate(['/setup/dropdown-add/' + id]);
    }


}
