import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { APIModel } from 'src/app/Shared/Entity';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
    selector: 'app-schemedetail-list',
    templateUrl: './schemedetail-list.component.html',
    styleUrls: ['./schemedetail-list.component.css']
})
export class SchemedetailListComponent implements OnInit {
    schemeDetailMasterList:any[] = []; 
    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    search: string;
    @ViewChild("paginator", { static: false }) paginator: Paginator;
    constructor(
        private router: Router,
        private alertService: AlertService,
        private schemeService: SchemeService) { this.pagingConfig = new APIModel(1, 10); }

    ngOnInit() {
        this.onSchemeDetailMasterList(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSchemeDetailMasterList(index,pageSize,search="") {
        this.alertService.fnLoading(true);
        this.schemeService.getSchemeDetailWithMaster(index,pageSize,search).subscribe(
            (res) => {
                debugger;
                this.pagingConfig=res.data
                this.schemeDetailMasterList = this.pagingConfig.model;;
              
            },
            () => { },
            () => { this.alertService.fnLoading(false);}
        )
    }
    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onSchemeDetailMasterList(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onSchemeDetailMasterList(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paginator.first = 1;
        this.pagingConfig = new APIModel(1, 10);
        // this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    isLastPage(): boolean {

        return this.schemeDetailMasterList ? this.first === (this.schemeDetailMasterList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.schemeDetailMasterList ? this.pagingConfig.pageNumber === 1 : true;
    }

    onSearch() {
        this.reset();
        this.onSchemeDetailMasterList(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    paginate(event) {
        //let first = Number(event.page) + 1;
        //this.OnLoadDealer(first, event.rows, this.search);
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.onSchemeDetailMasterList(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
        // event.first == 0 ?  1 : event.first;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
    
    add() {
        this.router.navigate(["/scheme/detail-add"]);
    }
    edit(id) {
        this.router.navigate(["/scheme/detail-add", id]);

    }
    delete(id) {

        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.alertService.fnLoading(true);
            this.schemeService.deleteSchemeDetail(id).subscribe(
                (res: any) => {
                    //  console.log('res from del func', res);
                    this.alertService.tosterSuccess("Scheme master has been deleted successfully.");
                    this.onSchemeDetailMasterList(this.pagingConfig.pageNumber,this.pagingConfig.pageSize,this.search);
                },
                (error) => {
                    console.log(error);
                }, () => () => this.alertService.fnLoading(false)
            );
        }, () => { });

    }
}
