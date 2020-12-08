import { Component, OnInit, ViewChild } from '@angular/core';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { FocusdealerService } from 'src/app/Shared/Services/FocusDealer/focusdealer.service';
import { APIModel } from '../../../Shared/Entity';

@Component({
    selector: 'app-dealer-list',
    templateUrl: './dealer-list.component.html',
    styleUrls: ['./dealer-list.component.css']
})
export class DealerListComponent implements OnInit {

    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    pageSize: number;
    search: string = "";
    dealerList: any[] = [];
    @ViewChild("paging", { static: false }) paging: Paginator;

    constructor(
        private dealerSvc: FocusdealerService,
        private alertSvc: AlertService
    ) {
        this.pagingConfig = new APIModel(1, 10);
    }

    ngOnInit() {
        this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);

    } next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paging.first = 1;
        this.pagingConfig = new APIModel(1, 10);
        // this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    isLastPage(): boolean {

        return this.dealerList ? this.first === (this.dealerList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.dealerList ? this.pagingConfig.pageNumber === 1 : true;
    }
    public paginate(event) {
        //let first = Number(event.page) + 1;
        //this.OnLoadDealer(first, event.rows, this.search);
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
        // event.first == 0 ?  1 : event.first;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
    OnLoadDealer(index, pageSize, search) {
        this.dealerSvc.getDealerList(index, pageSize, search).subscribe(
            (res: any) => {
                this.pagingConfig = res.data;
                this.dealerList = this.pagingConfig.model;

            },
            (error) => { this.displayError(error) },
        ).add(() => this.alertSvc.fnLoading(false))
    }

    onChange(value, dealer, property) {
        // if (property == 'isCBInstalled') dealer[property] = value;
        // if (property == 'isExclusive') dealer[property] = value;
        debugger;

        dealer[property] = value;
        this.alertSvc.fnLoading(true);

        this.dealerSvc.updateDealerStatus(dealer).subscribe(
            (res) => {
                this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
            },
            () => { }
        ).add(() => this.alertSvc.fnLoading(false));
    }
    private displayError(errorDetails: any) {
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertSvc.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertSvc.tosterDanger(errorDetails.error.msg);
        }
    }
}
