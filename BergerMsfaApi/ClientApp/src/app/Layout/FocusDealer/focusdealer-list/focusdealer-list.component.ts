import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ActivityPermissionService, PermissionGroup } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { FocusdealerService } from '../../../Shared/Services/FocusDealer/focusdealer.service';
import { FocusDealer } from '../../../Shared/Entity/FocusDealer/JourneyPlan';
import { APIModel } from '../../../Shared/Entity';
import { Paginator } from 'primeng/paginator';

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
    pagingConfig: APIModel;
    pageSize: number;
    search: string;
    @ViewChild("paginator", { static: false }) paginator: Paginator;
    ngOnInit() {
       
        this.pagingConfig = new APIModel(1, 10);
        this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);

    }

    private OnLoadFocusDealer(index, pageSize, search) {

        this.alertService.fnLoading(true);

        this.focusDealerService.getFocusdealerListPaging(index, pageSize, search)
            .subscribe(
                (res) => {
                    debugger;
                    this.pagingConfig = res.data;
                   // this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
                    this.focusDealerList = this.pagingConfig.model as [] || []
                },
                (error) => console.log(error)

            ).add(() => this.alertService.fnLoading(false));
    }
    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paginator.first = 1;
        this.pagingConfig = new APIModel(1, 10);
        this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    isLastPage(): boolean {

        return this.focusDealerList ? this.first === (this.focusDealerList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.focusDealerList ? this.first === 1 : true;
    }
    paginate(event) {
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        // event.first == 0 ?  1 : event.first;
      //  let first = Number(event.page) + 1;
        this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
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
                    this.OnLoadFocusDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
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

        //this.ptableSettings.enabledRecordCreateBtn = true;
        //this.ptableSettings.enabledEditBtn = true;
        //this.ptableSettings.enabledDeleteBtn = true;


    }

}
