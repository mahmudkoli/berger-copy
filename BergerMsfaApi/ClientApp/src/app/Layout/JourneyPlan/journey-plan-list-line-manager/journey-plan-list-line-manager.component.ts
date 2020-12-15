import { Component, OnInit, ViewChild } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { Router, ActivatedRoute } from '@angular/router';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { JourneyPlanStatus } from '../../../Shared/Entity/JourneyPlan/JourneyPlanStatus';
import { PlanStatus } from '../../../Shared/Enums/PlanStatus';
import { APIModel } from '../../../Shared/Entity';
import { Paginator } from 'primeng/paginator';


@Component({
  selector: 'app-journey-plan-list-line-manager',
  templateUrl: './journey-plan-list-line-manager.component.html',
  styleUrls: ['./journey-plan-list-line-manager.component.css']
})
export class JourneyPlanListLineManagerComponent implements OnInit {

    permissionGroup: PermissionGroup = new PermissionGroup();
    journeyPlanStatus: JourneyPlanStatus = new JourneyPlanStatus();
    public journeyPlanList: JourneyPlan[] = [];
    PlanStatusEnum = PlanStatus;
    statusKeys: any[] = [];
    first = 1;
    rows = 10;
    search: string = "";
    pagingConfig: APIModel;


    @ViewChild("paginator", { static: false }) paginator: Paginator;

    constructor(
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private journeyPlanService: JourneyPlanService
    ) {
        this.pagingConfig = new APIModel(1, 10);
        this._initPermissionGroup();
    }

    ngOnInit() {
        this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paginator.first = 1;
        this.pagingConfig = new APIModel(1, 10);
    }

    isLastPage(): boolean {

        return this.journeyPlanList ? this.first === (this.journeyPlanList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.journeyPlanList ? this.first === 1 : true;
    }
    paginate(event) {

        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);

    }
    onStatusChange(key, jPlan) {

        this.journeyPlanStatus.planId = jPlan.id;
        this.journeyPlanStatus.status = Number(key);
        this.alertService.confirm(`Are you sure to change status?`, () => {
            this.alertService.fnLoading(true);
            this.journeyPlanService.ChangePlanStatus(this.journeyPlanStatus).subscribe(
                (res) => {
                    this.alertService.tosterSuccess(`Status Successfully.`);
                  //  this.fnJourneyPlanList();
                    this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
                },
                (error) => {
                    console.log(error);
                    this.displayError(error);
                }

            ).add(() => this.alertService.fnLoading(false));
        }, () => {

        });
    }

    private onLoadLinemanagerJourneyPlans(index, pageSize, search) {

        this.alertService.fnLoading(true);

        this.journeyPlanService.getLinerManagerJourneyPlanList(index, pageSize, search)
            .subscribe(
                (res) => {
                    this.pagingConfig = res.data;
                   // this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
                    this.journeyPlanList = this.pagingConfig.model as [] || []
                },
                (error) => {
                    this.displayError(error);
                }

            ).add(() => this.alertService.fnLoading(false));
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

    onDetail(plan) {
        this.router.navigate(["/journey-plan/line-manager-detail/", plan.id]);
    }
    
     add() {
        this.router.navigate(['/journey-plan/add']);
    }

     edit(id: number) {
        console.log('edit plan', id);
        this.router.navigate(['/journey-plan/add/' + id]);
    }

     delete(id: number) {
        console.log("Id:", id);
         this.alertService.confirm("Are you sure you want to delete this item?", () => {
             this.alertService.fnLoading(true);
            this.journeyPlanService.delete(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("journey plan has been deleted successfully.");
               
                    this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
                },
                (error) => {
                    console.log(error);
                    this.displayError(error);
                }
            ).add(() => this.alertService.fnLoading(false));
        }, () => {

        });
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

}
