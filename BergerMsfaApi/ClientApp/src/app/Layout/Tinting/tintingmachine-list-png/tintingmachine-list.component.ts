import { Component, OnInit, ViewChild } from '@angular/core';
import { TintingService } from '../../../Shared/Services/Tinting/TintingService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { APIModel } from 'src/app/Shared/Entity/Response';
import { Paginator } from 'primeng/paginator';

@Component({
    selector: 'app-tintingmachine-png-list',
    templateUrl: './tintingmachine-list.component.html',
    styleUrls: ['./tintingmachine-list.component.css']
})
export class TintingmachineListPngComponent implements OnInit {
    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    pageSize: number;
    search: string = "";
    tintingMachineList: any[] = [];
    @ViewChild("paging", { static: false }) paging: Paginator;
    constructor(
        private alertService: AlertService,
        private tintingMachineSvc: TintingService) {
        this.pagingConfig = new APIModel(1, 10);
    }

    ngOnInit() {
        this.onLoadTintingMachines(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }




    onLoadTintingMachines(index, pageSize, search = "") {

        this.alertService.fnLoading(true);
        this.tintingMachineSvc.getTintingMachinePagingList(index, pageSize, search)
            .subscribe(
                (res) => {
                    this.pagingConfig = res.data;
                    this.tintingMachineList = this.pagingConfig.model;
                },
                (error) => { this.displayError(error) }

            ).add(() => this.alertService.fnLoading(false));
    }


    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onLoadTintingMachines(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onLoadTintingMachines(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.onLoadTintingMachines(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);

    }
    reset() {
        this.paging.first = 1;
        this.pagingConfig = new APIModel(1, 10);
    }

    isLastPage(): boolean {

        return this.tintingMachineList ? this.first === (this.tintingMachineList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.tintingMachineList ? this.first === (this.tintingMachineList.length - this.rows) : true;
    }
    paginate(event) {

        // event.first == 0 ?  1 : event.first;
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.onLoadTintingMachines(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
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
