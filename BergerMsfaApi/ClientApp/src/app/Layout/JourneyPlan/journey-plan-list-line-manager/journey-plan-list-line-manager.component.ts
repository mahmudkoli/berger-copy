import { Component, OnInit } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { Router, ActivatedRoute } from '@angular/router';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { Status } from '../../../Shared/Enums/status';
import { JourneyPlanStatus } from '../../../Shared/Entity/JourneyPlan/JourneyPlanStatus';

@Component({
  selector: 'app-journey-plan-list-line-manager',
  templateUrl: './journey-plan-list-line-manager.component.html',
  styleUrls: ['./journey-plan-list-line-manager.component.css']
})
export class JourneyPlanListLineManagerComponent implements OnInit {

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
            this.alertService.fnLoading(true);
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
        console.log(this.permissionGroup);
        //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;

        //this.ptableSettings.enabledRecordCreateBtn = true;
        //this.ptableSettings.enabledEditBtn = true;
        //this.ptableSettings.enabledDeleteBtn = true;

    }

    private fnJourneyPlanList() {

        this.alertService.fnLoading(true);
        this.journeyPlanService.getLinerManagerJourneyPlanList()
            .subscribe(
                (res) => {
                    this.journeyPlanList = res.data as [] || [];

                },
                (error) => {
                    console.log(error);
                },
                () => this.alertService.fnLoading(false)
            );
    }

    openModal(plan) {
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
            this.journeyPlanService.delete(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("journey plan has been deleted successfully.");
                    this.fnJourneyPlanList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }
}
