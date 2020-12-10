import { Component, OnInit, ViewChild } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { DealeropeningService } from '../../../Shared/Services/FocusDealer/dealeropening.service';
import { APIModel } from 'src/app/Shared/Entity';
import { Paginator } from 'primeng/paginator';

@Component({
    selector: 'app-dealer-opening-list',
    templateUrl: './dealer-opening-list.component.html',
    styleUrls: ['./dealer-opening-list.component.css']
})
export class DealerOpeningListComponent implements OnInit {
    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    search: string = "";
    public dealerOpeningList: any[] = [];
    @ViewChild("paginator", { static: false }) paginator: Paginator;
    constructor(
        private router: Router,
        private alertService: AlertService,
        private dealeropeningService: DealeropeningService
    ) {
        //  this._initPermissionGroup();
        this.pagingConfig = new APIModel(1, 10);

    }

    ngOnInit() {
        this.onLoadDealerOpenings(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onLoadDealerOpenings(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onLoadDealerOpenings(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.onLoadDealerOpenings(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paginator.first = 1;
        this.pagingConfig = new APIModel(1, 10);
        // this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    isLastPage(): boolean {

        return this.dealerOpeningList ? this.first === (this.dealerOpeningList.length - this.rows) : true;
    }
    isFirstPage(): boolean {
        return this.dealerOpeningList ? this.pagingConfig.pageNumber === 1 : true;
    }
    public paginate(event) {
        //let first = Number(event.page) + 1;
        //this.OnLoadDealer(first, event.rows, this.search);
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.onLoadDealerOpenings(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
        // event.first == 0 ?  1 : event.first;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
    onLoadDealerOpenings(index, pageSize, search="") {
        this.alertService.fnLoading(true);

        this.dealeropeningService.GetDealerOpeningList(index, pageSize, search).subscribe(
            (res) => {
                this.pagingConfig=res.data;
                this.dealerOpeningList = this.pagingConfig.model;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }


    detail(id) {
        this.router.navigate(['/dealer/openingList/' + id]);

    }

}
