import { Component, OnInit } from "@angular/core";
import { NgbDate, NgbCalendar, NgbDateParserFormatter } from "@ng-bootstrap/ng-bootstrap";
import { UserInfo } from "src/app/Shared/Entity/Users/userInfo";
import { UserService } from "src/app/Shared/Services/Users";
import { Delegation } from "src/app/Shared/Entity/Users/delegation";
import { AlertService } from "src/app/Shared/Modules/alert/alert.service";
import { ActivatedRoute, Router } from "@angular/router";
import { DelegationService } from "src/app/Shared/Services/Users/delegation.service";

@Component({
    selector: "app-delegation-add",
    templateUrl: "./delegation-add.component.html",
    styleUrls: ["./delegation-add.component.css"],
})
export class DelegationAddComponent implements OnInit {
    hoveredDate: NgbDate | null = null;

    fromDate: NgbDate | null;
    //toDate: NgbDate | null;


    delegationModel: Delegation;
    userInfoList: UserInfo[] = [];

    constructor(
        private calendar: NgbCalendar,
        public formatter: NgbDateParserFormatter,
        private userService: UserService,
        private alertService: AlertService,
        private route: ActivatedRoute,
        private delegationService: DelegationService,
        private router: Router
    ) {
        
    }

    ngOnInit() {
        this.createForm();

        if (
            Object.keys(this.route.snapshot.params).length !== 0 &&
            this.route.snapshot.params.id !== "undefined"
        ) {
            // console.log("id", this.route.snapshot.params.id);
            let delegationId = this.route.snapshot.params.id;
            this.getDelegation(delegationId);
        }
    }

    public fnChangeDdl(id) {
        console.log("fnChangeDdl id", id);
    }

    compareFn(user1: UserInfo, user2: UserInfo) {
        console.log("fnChangeDdl id", 123);
        return user1 && user2 ? user1.id === user2.id : user1 === user2;
    }

    getDelegation(delegationId) {
        this.delegationService.getDelegationById(delegationId).subscribe(
            (result: any) => {
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    }

    editForm(delegationData: Delegation) {
        var fdt = this.formatter.parse(delegationData.fromDate);
        var tdt = this.formatter.parse(delegationData.toDate);
        this.delegationModel.id = delegationData.id;
        this.delegationModel.name = delegationData.name;
        this.delegationModel.deligatedFromUserId = delegationData.deligatedFromUserId;
        this.delegationModel.deligatedToUserId = delegationData.deligatedToUserId;
        this.delegationModel.frombDate = NgbDate.from(fdt);
        this.delegationModel.tobDate = NgbDate.from(tdt);
    }

    getUserList() {
        this.userService.getAllUserInfo().subscribe((res: any) => {
            this.userInfoList = res.data || [];
        });
    }

    createForm() {
        this.delegationModel = new Delegation();
        this.getUserList();
        this.delegationModel.frombDate = this.calendar.getToday();
        this.delegationModel.tobDate = this.calendar.getNext(this.calendar.getToday(),"d",10);
    }

    saveDelegation(model: Delegation) {
        this.delegationModel.id == 0 ? this.insertDelegation(this.delegationModel) : this.updateDelegation(this.delegationModel);
    }

    private insertDelegation(model: Delegation) {
        let dtFrm = "";
        let dtTo = "";
        dtFrm = model.frombDate.year.toString() + "-" + model.frombDate.month.toString() + "-" + model.frombDate.day.toString();
        model.fromDate = dtFrm;
        dtTo = model.tobDate.year.toString() + "-" + model.tobDate.month.toString() + "-" + model.tobDate.day.toString();
        model.toDate = dtTo;
        this.delegationService.postDelegation(model).subscribe(
            (res) => {
                this.router.navigate(["/users/delegation-list"]).then(() => {
                    this.alertService.titleTosterSuccess(
                        "Delegation has been saved successfully."
                    );
                });
            },
            (error) => {
                this.displayError(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }


    private updateDelegation(model: Delegation) {
        let dtFrm = "";
        let dtTo = "";
        dtFrm = model.frombDate.year.toString() + "-" + model.frombDate.month.toString() + "-" + model.frombDate.day.toString();
        model.fromDate = dtFrm;
        dtTo = model.tobDate.year.toString() + "-" + model.tobDate.month.toString() + "-" + model.tobDate.day.toString();
        model.toDate = dtTo;
        this.delegationService.putDelegation(model).subscribe(
            (res) => {
                this.router.navigate(["/users/delegation-list"]).then(() => {
                    this.alertService.titleTosterSuccess(
                        "Delegation has been updated successfully."
                    );
                });
            },
            (error) => {
                this.displayError(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    
    displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
       
        let errList = errorDetails.error.errors;
        if (errList.length) {
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

    delegationList() {
        this.router.navigate(["/users/delegation-list"]);
    }

    onDateSelection(date: NgbDate) {
        if (!this.delegationModel.frombDate && !this.delegationModel.tobDate) {
            this.delegationModel.frombDate = date;
        } else if (
            this.delegationModel.frombDate &&
            !this.delegationModel.tobDate &&
            date &&
            date.after(this.delegationModel.frombDate)
        ) {
            this.delegationModel.tobDate = date;
        } else {
            this.delegationModel.tobDate = null;
            this.delegationModel.frombDate = date;
        }
    }

    isHovered(date: NgbDate) {
        return (
            this.delegationModel.frombDate &&
            !this.delegationModel.tobDate &&
            this.hoveredDate &&
            date.after(this.delegationModel.frombDate) &&
            date.before(this.hoveredDate)
        );
    }

    isInside(date: NgbDate) {
        return this.delegationModel.tobDate && date.after(this.delegationModel.frombDate) && date.before(this.delegationModel.tobDate);
    }

    isRange(date: NgbDate) {
        return (
            date.equals(this.delegationModel.frombDate) ||
            (this.delegationModel.tobDate && date.equals(this.delegationModel.tobDate)) ||
            this.isInside(date) ||
            this.isHovered(date)
        );
    }

    validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
        const parsed = this.formatter.parse(input);
        return parsed && this.calendar.isValid(NgbDate.from(parsed))
            ? NgbDate.from(parsed)
            : currentValue;
    }

    validateFromUser(event : any){

        debugger;

        if(event == this.delegationModel.deligatedToUserId)
        {
            this.delegationModel.deligatedFromUserId = null;
        }

    }

    validateToUser(event : any){

        debugger;

        if(this.delegationModel.deligatedFromUserId == event)
        {
            this.delegationModel.deligatedToUserId = null;
        }

    }
}

