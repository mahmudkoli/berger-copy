import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ActivityPermissionService, PermissionGroup } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { FocusdealerService } from '../../../Shared/Services/FocusDealer/focusdealer.service';
import { FocusDealer } from '../../../Shared/Entity/FocusDealer/JourneyPlan';

@Component({
  selector: 'app-focusdealer-list',
  templateUrl: './focusdealer-list.component.html',
  styleUrls: ['./focusdealer-list.component.css']
})
export class FocusdealerListComponent implements OnInit {

    permissionGroup: PermissionGroup = new PermissionGroup();
    public focusDealerList: FocusDealer[] = [];

    constructor(
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private focusDealerService: FocusdealerService
    ) {
        this._initPermissionGroup();
    }
    first = 1;
    rows = 10;
    planDate: string = "";
    pagingConfig: any;
    pageSize: number;
    searchDate: string;
    ngOnInit() {
        // this.fnFocusDealerList();
        this.getFocusdealerListPaging(this.first, this.rows, this.searchDate);

    }

    private getFocusdealerListPaging(index, pageSize, searchDate="") {

        this.alertService.fnLoading(true);

        this.focusDealerService.getFocusdealerListPaging(index, pageSize, this.searchDate="")
            .subscribe(
                (res) => {
                    debugger;
                    this.pagingConfig = res.data;
                    this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
                    this.focusDealerList = this.pagingConfig.model as [] || []
                },
                (error) => console.log(error)

            ).add(() => this.alertService.fnLoading(false));
    }
    next() {
        this.first = this.first + this.rows;
        this.getFocusdealerListPaging(this.first, this.rows, this.searchDate);
    }

    prev() {
        this.first = this.first - this.rows;
        this.getFocusdealerListPaging(this.first, this.rows, this.searchDate);
    }
    onSearch() {
        this.getFocusdealerListPaging(this.first, this.rows, this.searchDate);
    }
    reset() {
        this.first = 1;
        this.getFocusdealerListPaging(this.first, this.rows, this.searchDate);
    }

    isLastPage(): boolean {

        return this.focusDealerList ? this.first === (this.focusDealerList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.focusDealerList ? this.first === 1 : true;
    }
    paginate(event) {

        // event.first == 0 ?  1 : event.first;
        let first = Number(event.page) + 1;
        this.getFocusdealerListPaging(first, event.rows);
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
     fnFocusDealerList() {
        this.alertService.fnLoading(true);

        this.focusDealerService.getFocusDealerList().subscribe(
            (res) => {
                           this.focusDealerList = res.data || [];;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
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
    } private add() {
        this.router.navigate(['/dealer/add-focusdealer']);
    }

    private edit(id: number) {
        console.log('edit plan', id);
        this.router.navigate(['/dealer/add-focusdealer/' + id]);
    }

    private delete(id: number) {
        console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.focusDealerService.delete(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("dropdown has been deleted successfully.");
                    //this.fnFocusDealerList();
                    this.getFocusdealerListPaging(this.first, this.rows, this.searchDate);
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
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
    public ptableSettings = {
        tableID: "Focus-Dealer",
        tableClass: "table table-border ",
        tableName: 'Focus Dealer List',
        tableRowIDInternalName: "Id",
        tableColDef: [

            { headerName: 'Dealer ', width: '10%', internalName: 'dealerName', sort: true, type: "" },
            { headerName: 'Employee', width: '30%', internalName: 'employeeName', sort: true, type: "" },
            { headerName: 'Valid From', width: '30%', internalName: 'validFrom', sort: true, type: "" },
            { headerName: 'Valid To', width: '20%', internalName: 'validTo', sort: true, type: "" },


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
