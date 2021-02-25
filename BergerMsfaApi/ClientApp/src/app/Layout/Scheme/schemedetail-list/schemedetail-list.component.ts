import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { APIModel } from 'src/app/Shared/Entity';
import { SchemeDetail, SchemeDetailQuery } from 'src/app/Shared/Entity/Scheme/SchemeMaster';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
    selector: 'app-schemedetail-list',
    templateUrl: './schemedetail-list.component.html',
    styleUrls: ['./schemedetail-list.component.css']
})
export class SchemedetailListComponent implements OnInit, OnDestroy {

	query: SchemeDetailQuery;
	PAGE_SIZE: number;
	schemeDetails: SchemeDetail[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private schemeDetailService: SchemeService,
		private modalService: NgbModal,
		private commonService: CommonService) {
			// this.PAGE_SIZE = 5000;
			// this.ptableSettings.pageSize = 10;
			// this.ptableSettings.enabledServerSitePaggination = false;
			// server side paggination
			this.PAGE_SIZE = commonService.PAGE_SIZE;
			this.ptableSettings.pageSize = this.PAGE_SIZE;
			this.ptableSettings.enabledServerSitePaggination = true;
	}

	ngOnInit() {
		this.searchConfiguration();
		of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
			this.loadSchemeDetailsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadSchemeDetailsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const schemeDetailsSubscription = this.schemeDetailService.getSchemeDetails(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.schemeDetails = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.schemeDetails.forEach(obj => {
						obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(schemeDetailsSubscription);
	}

	searchConfiguration() {
		this.query = new SchemeDetailQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'title',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.schemeDetailService.activeInactive(id).subscribe(res => {
	// 		this.loadSchemeDetailsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editSchemeDetail(id) {
		this.router.navigate(['/scheme/detail-edit', id]);
	}

	newSchemeDetail() {
		this.router.navigate(['/scheme/detail-add']);
	}

	deleteSchemeDetail(id) {
		this.alertService.confirm("Are you sure want to delete this Scheme Detail?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.schemeDetailService.deleteSchemeDetail(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Scheme Detail has been deleted successfully.");
						this.loadSchemeDetailsPage();
					},
						(error) => {
							console.log(error);
						});
				this.subscriptions.push(deleteSubscription);
			},
			() => {
			});
	}

	public ptableSettings: IPTableSetting = {
		tableID: "schemeDetails-table",
		tableClass: "table table-border ",
		tableName: 'Scheme Detail List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Scheme Master', width: '15%', internalName: 'schemeMasterName', sort: true, type: "" },
			{ headerName: 'SM Condition', width: '15%', internalName: 'schemeMasterCondition', sort: false, type: "" },
			{ headerName: 'Brand', width: '15%', internalName: 'brand', sort: false, type: "" },
			{ headerName: 'SLAB', width: '10%', internalName: 'slab', sort: false, type: "" },
			{ headerName: 'Product', width: '15%', internalName: 'material', sort: false, type: "" },
			{ headerName: 'Benefit', width: '20%', internalName: 'benefit', sort: false, type: "" },
			{ headerName: 'Status', width: '10%', internalName: 'statusText', sort: false, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDeleteBtn: true,
		enabledEditBtn: true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: true,
		enabledDataLength: true,
		newRecordButtonText: 'New Scheme Detail'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newSchemeDetail();
		}
		else if (event.action == "edit-item") {
			this.editSchemeDetail(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteSchemeDetail(event.record.id);
		}
	}
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new SchemeDetailQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadSchemeDetailsPage();
	}
}

