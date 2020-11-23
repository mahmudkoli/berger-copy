import { Component, OnInit } from '@angular/core';
import { TintingService } from '../../../Shared/Services/Tinting/TintingService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { CommonService } from '../../../Shared/Services/Common/common.service';

@Component({
  selector: 'app-tintingmachine-list',
  templateUrl: './tintingmachine-list.component.html',
  styleUrls: ['./tintingmachine-list.component.css']
})
export class TintingmachineListComponent implements OnInit {

    tintingMachineList: any[] = [];
    constructor(
        private alertService: AlertService,
        private commonSvc: CommonService,
        private tintingMachineSvc: TintingService) { }

    ngOnInit() {
       // this.getTintingMachineList();
    }

    first = 1;
    rows = 10;
    planDate: string = "";
    pagingConfig: any;
    pageSize: number;

    private get _LoggedUser() { return this.commonSvc.getUserInfoFromLocalStorage() }
  
    getTintingMachinePagingList(index, pageSize, companyName) {
        if (this._LoggedUser && this._LoggedUser.userCategory=="Terriotry") {

            this.alertService.fnLoading(true);
            this.tintingMachineSvc.getTintingMachinePagingList(this._LoggedUser.userCategoryIds, index, pageSize, companyName)
                .subscribe(
                    (res) => {
                        this.pagingConfig = res.data;
                        this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
                        this.tintingMachineList = this.pagingConfig.model as [] || []
                    },
                    (error) => { this.displayError(error) }

                ).add(() => this.alertService.fnLoading(false));
        }
       
    }
    next() {
        this.first = this.first + this.rows;
        this.getTintingMachinePagingList(this.first, this.rows, this.planDate);
    }

    prev() {
        this.first = this.first - this.rows;
        this.getTintingMachinePagingList(this.first, this.rows, this.planDate);
    }
    onSearch() {

        this.getTintingMachinePagingList(this.first, this.rows, this.planDate);
    }
    reset() {
        this.first = 1;
        this.getTintingMachinePagingList(this.first, this.rows, this.planDate);
    }

    isLastPage(): boolean {

        return this.tintingMachineList ? this.first === (this.tintingMachineList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.tintingMachineList ? this.first === 1 : true;
    }
    paginate(event) {

        // event.first == 0 ?  1 : event.first;
        let first = Number(event.first) + 1;
        this.getTintingMachinePagingList(first, event.rows, this.planDate);
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
    private displayError(errorDetails: any) {
        this.alertService.fnLoading(false);
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
