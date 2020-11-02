import { Component, OnInit } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { Status } from '../../../Shared/Enums/status';
import { JourneyPlanStatus } from '../../../Shared/Entity/JourneyPlan/JourneyPlanStatus';



@Component({
    templateUrl: './journey-plan-list.component.html',
    styleUrls: ['./journey-plan-list.component.css']
})

export class JourneyPlanListComponent implements OnInit {

    permissionGroup: PermissionGroup = new PermissionGroup();
    journeyPlanStatus: JourneyPlanStatus = new JourneyPlanStatus();
    public journeyPlanList: JourneyPlan[] = [];
    changeStatus = Status;
    statusKeys: any[] = [];
   
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
        this.statusKeys = Object.keys(this.changeStatus).filter(k => !isNaN(Number(k)));
        this.fnJourneyPlanList();

    }
    onStatusChange(key, jPlan) {

        this.journeyPlanStatus.planId = jPlan.id;
        this.journeyPlanStatus.status = Number(key);

        this.alertService.confirm(`Are you sure to change status?`, () => {

            this.journeyPlanService.ChangePlanStatus(this.journeyPlanStatus).subscribe(
                (res) => {
                    this.alertService.tosterSuccess(`Status Successfully.`);
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
    }

    private fnJourneyPlanList() {

        this.alertService.fnLoading(true);
        this.journeyPlanService.getJourneyPlanList()
            .subscribe(
                (res) => {
                    this.journeyPlanList =res.data as [] || [];

                },
                (error) => {
                    console.log(error);
                },
                () => this.alertService.fnLoading(false)
            );
    }

    detail(plan) {
        this.router.navigate(["/journey-plan/detail", plan.id]);
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
                    this.fnJourneyPlanList();
                },
                (error) => {
                    console.log(error);
                }, () => () => this.alertService.fnLoading(false)
            );
        }, () => {

        });
    }

}
