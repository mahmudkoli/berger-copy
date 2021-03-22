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
import { CalendarOptions, DateSelectArg } from '@fullcalendar/angular';
import { FullCalendar } from 'primeng-lts/fullcalendar';


@Component({
    selector: 'app-journey-plan-list-line-manager',
    templateUrl: './journey-plan-list-line-manager.component.html',
    styleUrls: ['./journey-plan-list-line-manager.component.css']
})
export class JourneyPlanListLineManagerComponent implements OnInit {

    calendarOptions: CalendarOptions = {};
    permissionGroup: PermissionGroup = new PermissionGroup();
    journeyPlanStatus: JourneyPlanStatus = new JourneyPlanStatus();
    public journeyPlanList: any[] = [];
    PlanStatusEnum = PlanStatus;
    statusKeys: any[] = [];
    first = 1;
    rows = 10;
    search: string = "";
    pagingConfig: APIModel;


    @ViewChild("paginator", { static: false }) paginator: Paginator;
    @ViewChild('fc', { static: false }) fc: FullCalendar;
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
        this.calendarOptions = {
            headerToolbar: {
                left: 'prev,next,today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            initialView: 'dayGridMonth',
            // dateClick: this.handleDateClick.bind(this),
            // select: this.handleDateSelect.bind(this),
            eventClick: this.handleEventClick.bind(this),
            // eventsSet: this.handleEvents.bind(this),
            dayCellDidMount: function (info) {

                //     let view=document.createElement('button');
                //     view.className="btn btn-sm btn-primary";
                //     view.textContent="View";
                //     info.el.appendChild(view);



                //     let del=document.createElement('button');
                //     del.textContent="Delete";
                //     del.className="btn btn-sm btn-primary";
                //     info.el.appendChild(del);
                //     del.addEventListener("click",function(){
                //         alert("Delete");
                //     })
                //     // info.el.innerHTML=`<button class='btn btn-sm btn-primary'>View</button>`;
                //     // info.el.innerHTML=`<button class='btn btn-sm btn-primary'>Delete</button>`;
                //    return info.el;

            },

            eventContent: function (arg) {
                // debugger;

                // let italicEl = document.createElement('i')

                // if (arg.event.extendedProps.isUrgent) {
                //     italicEl.innerHTML = 'urgent event'
                // } else {
                //     italicEl.innerHTML = 'normal event'
                // }

                // let arrayOfDomNodes = [italicEl]
                // return { domNodes: arrayOfDomNodes }
            },
            // eventClick: this.handleEventClick.bind(this),
            // select: this.handleDateSelect.bind(this),
            //sevents: this.eventPlans,
            // initialEvents:this.journeyPlanList.map((f:any)=>({title:f.employeeName,date:f.planDate}))

            selectable: true,

            eventOrder: ['id'],




            // eventClick: this.handleEventClick.bind(this),
            //  eventsSet: this.handleEvents.bind(this)
            //eventClick:this.handleEventClick.bind(this)


        }





        this.onLoadLinemanagerJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    handleEventClick(info) {
        // debugger;
        // alert('Event: ' + info.event.startStr);
        // alert('Coordinates: ' + info.jsEvent.pageX + ',' + info.jsEvent.pageY);
        // alert('View: ' + info.view.type);
       // let eve=(this.calendarOptions.events as []).find(f => f.date == info.event.startStr);
       if (info.event.title == "View") {
        
           let find = this.journeyPlanList.find(f => f.id==info.event.id && f.planDate == info.event.startStr);
           this.onDetail(find);
       }
        
        // change the border color just for fun
        //info.el.style.borderColor = 'red';
        //  alert('event click! ' + arg.dateStr)
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
                    this.journeyPlanList = this.pagingConfig.model as [] || []
                    let events = [];
                    this.journeyPlanList.forEach(plan => {
                        events.push({ id: plan.id, title: `${plan.employeeName}`, date: plan.planDate, employeeId: plan.employeeId, backgroundColor: '#f58442' });
                        events.push({ id: plan.id, title: `${plan.planStatusInText}`, date: plan.planDate, employeeId: plan.employeeId });
                        events.push({ id: plan.id, title: 'View', date: plan.planDate,employeeId:plan.employeeId, backgroundColor: '#ce42f5' });

                    });

                    this.calendarOptions.events = [...events];

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
