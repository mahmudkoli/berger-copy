import { Component, OnInit } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { IPTableSetting } from '../../../Shared/Modules/p-table';



@Component({
    templateUrl: './journey-plan-list.component.html',
    styleUrls: ['./journey-plan-list.component.css']
})

export class JourneyPlanListComponent implements OnInit {

    permissionGroup: PermissionGroup = new PermissionGroup();
    public journeyPlanList: JourneyPlan[] = [];


    constructor(
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private journeyPlanService: JourneyPlanService
    ) {
        this._initPermissionGroup();
    }

    ngOnInit() {
        this.fnJourneyPlanList();
      
    }
    onStatusChange(model: JourneyPlan) {

        let message = model.approvedStatus?"Pend":"Approved"
        this.alertService.confirm(`Are you sure to ${message} this journey plan?`, () => {

            this.journeyPlanService.approveJournetPlan(model).subscribe(
                (res) => {
                    this.alertService.tosterSuccess(`Plan ${message} successfully.`);
                    this.fnJourneyPlanList();
                },
                (error) => {
                    console.log(error);
                },
                () => this.alertService.fnLoading(false)
            )
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

    private fnJourneyPlanList() {

        this.alertService.fnLoading(true);
        this.journeyPlanService.getJourneyPlanList()
            .subscribe(
                (res) => {
                    this.journeyPlanList = res.data as JourneyPlan[] || [];
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
    }

    private add() {
        this.router.navigate(['/journey-plan/add']);
    }

    private edit(id: number) {
        console.log('edit plan', id);
        this.router.navigate(['/journey-plan/add/' + id]);
    }

    private delete(id: number) {
        console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.journeyPlanService.delete(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("dropdown has been deleted successfully.");
                    this.fnJourneyPlanList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public ptableSettings: IPTableSetting = {
        tableID: "Journey-Plan",
        tableClass: "table table-border ",
        tableName: 'Journey Plan List',
        tableRowIDInternalName: "Id",
        tableColDef: [

            { headerName: 'Code ', width: '10%', internalName: 'code', sort: true, type: "" },
            { headerName: 'Employee', width: '30%', internalName: 'employeeRegId', sort: true, type: "" },
            { headerName: 'Visit Date', width: '30%', internalName: 'visitDate', sort: true, type: "" },
            //{ headername: 'approved status ', width: '20%', internalname: 'status', sort: true, type: "", innerbtnicon= },
            { headerName: 'Approved Status', width: '15%', internalName: 'approvedStatus', sort: true, type: "button", onClick: "", innerBtnIcon: "" },

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


