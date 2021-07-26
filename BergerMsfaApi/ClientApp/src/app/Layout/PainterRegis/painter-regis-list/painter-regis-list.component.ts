import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { Painter } from '../../../Shared/Entity/Painter/Painter';

import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { PainterRegisService } from '../../../Shared/Services/Painter-Regis/painterRegister.service';
import { APIModel } from 'src/app/Shared/Entity';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-painter-regis-list',
    templateUrl: './painter-regis-list.component.html',
    styleUrls: ['./painter-regis-list.component.css']
})
export class PainterRegisListComponent implements OnInit {

    public painterList: Painter[] = [];
    public baseUrl: string;

    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    search: string = "";
 @ViewChild("paging", { static: false }) paging: Paginator;
    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private router: Router,
        private painterRegisSvc: PainterRegisService,
        private alertService: AlertService,
        private modalService: NgbModal,
    ) {
        this.baseUrl = baseUrl;
        this.pagingConfig = new APIModel(1, 10);
    }

    ngOnInit() {
        this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    next() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
        this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    prev() {
        this.pagingConfig.pageNumber = this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
        this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    onSearch() {
        this.reset();
        this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }
    reset() {
        this.paging.first = 1;
        this.pagingConfig = new APIModel(1, 10);
        // this.OnLoadDealer(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
    }

    isLastPage(): boolean {

        return this.painterList ? this.first === (this.painterList.length - this.rows) : true;
    }

    isFirstPage(): boolean {
        return this.painterList ? this.pagingConfig.pageNumber === 1 : true;
    }
    public paginate(event) {
        //let first = Number(event.page) + 1;
        //this.OnLoadDealer(first, event.rows, this.search);
        this.pagingConfig.pageNumber = Number(event.page) + 1;
        this.pagingConfig.pageSize = Number(event.rows);
        this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
        // event.first == 0 ?  1 : event.first;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
    }
    detail(id) {
        this.router.navigate(['/painter/detail/' + id]);
    }
    updateStatus(id) {
        this.router.navigate(['/painter/update/' + id]);
    }
    
    updatePainterStatus(painterId: number) {
		this.alertService.fnLoading(true);
		const painterStatus = this.painterRegisSvc.UpdatePainterStatus(painterId)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
				},
				(error) => {
					console.log(error);
				});
	}

    onLoadPainters(index,pageSize,search="") {
        this.alertService.fnLoading(true);

        this.painterRegisSvc.GetPainterList(index,pageSize,search).subscribe(
            (res) => {
                this.pagingConfig=res.data;
                this.painterList = this.pagingConfig.model;
                this.painterList.forEach(obj => {
                    obj.statusText = (obj.status == 1) ? 'Active' : 'Inactive';
                    obj.statusBtnClass = 'btn-transition btn btn-sm btn-outline-' + ((obj.status == 1) ? 'primary' : 'warning') + ' d-flex align-items-center';
                    obj.statusBtnIcon = 'pr-1 fa fa-' + ((obj.status == 1) ? 'check' : 'ban');
                });
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }
}
