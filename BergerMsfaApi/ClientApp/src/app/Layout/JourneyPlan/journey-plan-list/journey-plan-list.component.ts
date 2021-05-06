import { Component, OnInit, ViewChild } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { JourneyPlanService } from '../../../Shared/Services/JourneyPlan/journey-plan.service';
import { JourneyPlan } from '../../../Shared/Entity/JourneyPlan/JourneyPlan';
import { JourneyPlanStatus } from '../../../Shared/Entity/JourneyPlan/JourneyPlanStatus';
import { PlanStatus } from '../../../Shared/Enums/PlanStatus';
import { APIModel } from '../../../Shared/Entity';
import { Paginator } from 'primeng/paginator';
import { CalendarOptions, DateSelectArg } from '@fullcalendar/angular';
import { FullCalendar } from 'primeng-lts/fullcalendar';
import { AuthService } from 'src/app/Shared/Services/Users';


@Component({
    templateUrl: './journey-plan-list.component.html',
    styleUrls: ['./journey-plan-list.component.css']
})

export class JourneyPlanListComponent implements OnInit {


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
    pageSize: number;
    @ViewChild("paginator", { static: false }) paginator: Paginator;
    @ViewChild('fc', { static: false }) fc: FullCalendar;

    constructor(

        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private journeyPlanService: JourneyPlanService,
        private authService: AuthService
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
            dateClick: this.handleDateClick.bind(this),
            //select: this.handleDateSelect.bind(this),
            //  eventClick: this.handleEventClick.bind(this),
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
            //eventColor: '#378006',
            eventOrder: ['id'],
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
            eventClick: this.handleEventClick.bind(this),
            // select: this.handleDateSelect.bind(this),
            //sevents: this.eventPlans,
            // initialEvents:this.journeyPlanList.map((f:any)=>({title:f.employeeName,date:f.planDate}))

            selectable: true,






            // eventClick: this.handleEventClick.bind(this),
            //  eventsSet: this.handleEvents.bind(this)
            //eventClick:this.handleEventClick.bind(this)
            // customButtons: {
            //     'new': {
            //         text: 'New Record',
            //         click: this.add.bind(this)
            //     }

            // }


        }
        // this.statusKeys = Object.keys(this.changeStatus).filter(k => !isNaN(Number(k)));
        this.onLoadJourneyPlans(this.first, this.rows, this.search);
        // this.calendarOptions.customButtons={
        //     new:{
        //         text:"New Record",
        //         click:function(){ 
        //             alert("Call");
        //           return  this.router.navigate(['/journey-plan/add']);
        //         }
        //     }
        // }


    }
    handleEvents(arg) {
        // debugger;
        alert('Set click! ' + arg.dateStr)
    }
    handleEventClick(info) {
        // debugger;
        let find = this.journeyPlanList.find(f => f.planDate == info.event.startStr);
        // this.detail(find);
        // alert('Cell');
        if (info.event.title == "Delete") {
            this.delete(find);
        }
        else if (info.event.title == "Edit") {
            this.edit(find);
        }
        else if (info.event.title == "View") {
            this.detail(find)
        }
       

        // alert('Event: ' + info.event.startStr);
        // alert('Coordinates: ' + info.jsEvent.pageX + ',' + info.jsEvent.pageY);
        // alert('View: ' + info.view.type);
        // let find = this.journeyPlanList.find(f => f.planDate == info.event.startStr);
        // this.detail(find);
        // change the border color just for fun
        //info.el.style.borderColor = 'red';
        //  alert('event click! ' + arg.dateStr)
    }
    handleDateClick(arg) {
        // debugger;
        var utc = new Date().toJSON().slice(0, 10).replace(/-/g, '-');
        // if (arg.dateStr >= utc) {
        if (this.compareDate(arg.dateStr)) {
            this.add(arg);
        }
        // let find = this.journeyPlanList.find(f => f.planDate == arg.dateStr);
        // this.edit(find);
    }
    handleDateSelect(selectInfo: DateSelectArg) {
        // debugger;
        let eventDate = selectInfo.view.currentStart;
        let find = this.journeyPlanList.find(f => f.planDate == eventDate);
        this.detail(find);


    }
    compareDate(pDate) {
        let pd = new Date(Date.parse(pDate));
        var planDate = pd.getFullYear() + "-" + (pd.getMonth() + 1) + "-" + pd.getDate() + " " + 0 + ":" + 0 + ":" + 0;
        var d = new Date();
        var currentDate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate() + " " + 0 + ":" + 0 + ":" + 0;
        var planDateInMileSeconds = Date.parse(planDate);
        var currentDateInMileScondes = Date.parse(currentDate);
        if (planDateInMileSeconds >= currentDateInMileScondes) return true;
        else return false
    }

    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onLoadJourneyPlans(this.first, this.rows, this.search);
    }

    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onLoadJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.onLoadJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paginator.first = 1;
        this.pagingConfig = new APIModel(1, 10);

    }

    isLastPage(): boolean {

        return this.journeyPlanList ? this.first === (this.journeyPlanList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.journeyPlanList ? this.pagingConfig.pageNumber === 1 : true;
    }
    public paginate(event) {

        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);

        this.onLoadJourneyPlans(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);

        // event.first == 0 ?  1 : event.first;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }

    private _initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    }


    private onLoadJourneyPlans(index, pageSize, search) {

        this.alertService.fnLoading(true);

        this.journeyPlanService.getJourneyPlanListPaging(index, pageSize, search)
            .subscribe(
                (res) => {
                    this.pagingConfig = res.data;
                    //  this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
                    this.journeyPlanList = this.pagingConfig.model as [] || []
                    let events = [];
                    this.journeyPlanList.forEach(plan => {
                        // debugger;
                        //  this.eventPlans=[...this.eventPlans,{ title: plan.employeeName, date: plan.planDate }]
                        events.push({ id: plan.id, title: plan.planStatusInText, date: plan.planDate });
                        events.push({ id: plan.id, title: 'View', date: plan.planDate, backgroundColor: '#ce42f5' });
                        events.push({ id: plan.id, title: 'Edit', date: plan.planDate, backgroundColor: '#f58442' });
                        events.push({ id: plan.id, title: 'Delete', date: plan.planDate, backgroundColor: '#f54272' });
                    });
                    events.sort()
                    this.calendarOptions.events = [...events];
                    // this.eventPlans=[...events];
                    // this.calendarOptions = {
                    //     headerToolbar: {
                    //         left: 'prev,next today',
                    //         center: 'title',
                    //         right: 'dayGridMonth,timeGridWeek,timeGridDay'
                    //     },
                    //     initialView: 'dayGridMonth',
                    //     // dateClick: this.handleDateClick.bind(this), 
                    //     select: this.handleDateSelect.bind(this),
                    //     //sevents: this.eventPlans,
                    //    // initialEvents:this.journeyPlanList.map((f:any)=>({title:f.employeeName,date:f.planDate}))



                    // }

                    // this.calendarOptions.customButtons = {
                    //     new: {
                    //         text: "New Record",
                    //         click:this.add.bind(this)
                    //     }
                    // }
                },
                (error) => this.displayError(error)

            ).add(() => this.alertService.fnLoading(false));
    }

    detail(plan) {
        this.router.navigate(["/journey-plan/detail", plan.id]);
    }

    add(arg) {

        let find = this.journeyPlanList.find(f => f.planDate == arg.dateStr);
        if(find) {
            this.alertService.alert("Already have a journey plan.");
            return;
        }

        if (this.authService.isAdmin) {
            this.alertService.alert("Admin can not create journey plan.");
            return;
        }

        this.router.navigate(['/journey-plan/add',arg.dateStr]);
    }

    edit(jPlan) {

        // debugger;
        if (this.compareDate(jPlan.planDate)) {
            console.log('edit plan', jPlan.id);

            if (jPlan.planStatus==PlanStatus.Approved) {
                this.alertService.alert("Can not modify approved plan.");
                return;
            }

            if (jPlan.planStatus==PlanStatus.Pending || jPlan.planStatus==PlanStatus.Edited) {
                this.alertService.alert("Already waiting for approval. So can not modify before reject the plan.");
                return;
            }

            if (jPlan.editCount>=2) {
                this.alertService.alert("Can not modify more than 2 times.");
                return;
            }

            this.router.navigate(['/journey-plan/add/' + jPlan.planDate]);
        }
        else this.alertService.alert("Can not modify pervious plan.");
    }

    delete(jPlan) {
        console.log("Id:", jPlan.id);

        if (this.compareDate(jPlan.planDate)) {

            if (jPlan.planStatus==PlanStatus.Approved) {
                this.alertService.alert("Can not delete approved plan.");
                return;
            }

            this.alertService.confirm("Are you sure you want to delete this item?", () => {
                this.alertService.fnLoading(true);
                this.journeyPlanService.delete(jPlan.id).subscribe(
                    (res: any) => {
                        console.log('res from del func', res);
                        this.alertService.tosterSuccess("Journey plan has been deleted successfully.");

                        this.onLoadJourneyPlans(this.first, this.rows, this.search);
                    },
                    (error) => {
                        console.log(error);
                        this.displayError(error);
                    }
                ).add(() => this.alertService.fnLoading(false));;
            }, () => {

            });
        }
        else this.alertService.alert("Can not delete pervious plan.");

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
