import { Component, OnInit, ViewChild } from '@angular/core';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { APIModel } from 'src/app/Shared/Entity';

@Component({
    selector: 'app-schememaster-png-list',
    templateUrl: './schememaster-list.component.html',
    styleUrls: ['./schememaster-list.component.css']
})
export class SchememasterListPngComponent implements OnInit {

    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    search: string;
    schemeMasterList: any[] = [];

    @ViewChild("paginator", { static: false }) paginator: Paginator;

    constructor(
        private schemeService: SchemeService,
        private alertService: AlertService,
        private router: Router
    ) {
        this.pagingConfig = new APIModel(1, 10);
    }



    ngOnInit() {
        this.onLoadSchemeMasters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);

    }


    onLoadSchemeMasters(index, pageSize, search) {
        this.alertService.fnLoading(true);
        this.schemeService.getSchemeMasterList(index, pageSize, search).subscribe((res) => {
            this.pagingConfig = res.data;
            this.schemeMasterList = this.pagingConfig.model;
        }, () => { }, () => this.alertService.fnLoading(false));
    }
    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onLoadSchemeMasters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onLoadSchemeMasters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paginator.first = 1;
        this.pagingConfig = new APIModel(1, 10);
        // this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    isLastPage(): boolean {

        return this.schemeMasterList ? this.first === (this.schemeMasterList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.schemeMasterList ? this.pagingConfig.pageNumber === 1 : true;
    }
    add() {
        this.router.navigate(["scheme/master-add",]);
    }
    edit(id) {
        this.router.navigate(["scheme/master-edit", id]);

    }
    onSearch() {
        this.reset();
        this.onLoadSchemeMasters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    paginate(event) {
        //let first = Number(event.page) + 1;
        //this.OnLoadDealer(first, event.rows, this.search);
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.onLoadSchemeMasters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
        // event.first == 0 ?  1 : event.first;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
    
    delete(id) {

        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.alertService.fnLoading(true);
            this.schemeService.deleteSchemeMaster(id).subscribe(
                (res: any) => {
                    this.alertService.tosterSuccess("Scheme master has been deleted successfully.");
                    this.onLoadSchemeMasters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
                },
                (error) => {
                    console.log(error);
                }, () => () => this.alertService.fnLoading(false)
            );
        }, () => { });

    }
    detail(id) {
        this.router.navigate(["scheme/master-edit", id]);
    }
}
