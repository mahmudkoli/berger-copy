import { Component, OnInit } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { Router, ActivatedRoute } from '@angular/router';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { JourneyPlanStatus } from '../../../Shared/Entity/JourneyPlan/JourneyPlanStatus';
import { PlanStatus } from '../../../Shared/Enums/PlanStatus';


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
    planDate: string = "";
    pagingConfig: any;
    pageSize: number;
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
       // this.statusKeys = Object.keys(this.changeStatus).filter(k => !isNaN(Number(k)));
       // this.statusKeys = Object.keys(this.changeStatus).filter(k => !isNaN(Number(k)));
        this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
      //  this.fnJourneyPlanList();

    }
    next() {
        this.first = this.first + this.rows;
        this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
    }

    prev() {
        this.first = this.first - this.rows;
        this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
    }
    onSearch() {

        this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
    }
    reset() {
        this.first = 1;
        this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
    }

    isLastPage(): boolean {

        return this.journeyPlanList ? this.first === (this.journeyPlanList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.journeyPlanList ? this.first === 1 : true;
    }
    paginate(event) {

        // event.first == 0 ?  1 : event.first;
        let first = Number(event.first) + 1;
        this.fnJourneyPlanListPaging(first, event.rows, this.planDate);
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
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
                    this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
                },
                (error) => {
                    console.log(error);
                    this.displayError(error);
                }

            ).add(() => this.alertService.fnLoading(false));
        }, () => {

        });
    }
    private fnJourneyPlanListPaging(index, pageSize, planDate) {

        this.alertService.fnLoading(true);

        this.journeyPlanService.getLinerManagerJourneyPlanList(index, pageSize, planDate)
            .subscribe(
                (res) => {
                    this.pagingConfig = res.data;
                    this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
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

    //private fnJourneyPlanList() {

    //    this.alertService.fnLoading(true);
    //    this.journeyPlanService.getLinerManagerJourneyPlanList()
    //        .subscribe(
    //            (res) => {
    //                this.journeyPlanList = res.data as [] || [];

    //            },
    //            (error) => {
    //                console.log(error);
    //            }
    //        ).add(() => this.alertService.fnLoading(false));
    //}

    openModal(plan) {
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
                  //  this.fnJourneyPlanList();
                    this.fnJourneyPlanListPaging(this.first, this.rows, this.planDate);
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
        // this.alertService.fnLoading(false);
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
