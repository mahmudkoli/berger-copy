import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { of, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { LeadSummaryQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';

@Component({
    selector: 'app-lead-summary-report',
    templateUrl: './lead-summary-report.component.html',
    styleUrls: ['./lead-summary-report.component.css']
})
export class LeadSummaryReportComponent implements OnInit, OnDestroy {

	query: LeadSummaryQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	tableName: string = 'Lead Summary Report';

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private reportService: ReportService,
		private modalService: NgbModal,
		private commonService: CommonService) {
			// client side paggination
			// this.PAGE_SIZE = 2147483647; // Int32 max value
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
			this.loadReportsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadReportsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const reportsSubscription = this.reportService.getLeadSummary(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.data = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					
					const obj = this.data[0] || {};
					this.ptableSettings.tableColDef = Object.keys(obj).map((key) => {
						return { headerName: this.commonService.insertSpaces(key), internalName: key } as colDef;
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(reportsSubscription);
	}
	
	searchConfiguration() {
		this.query = new LeadSummaryQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.reportService.activeInactive(id).subscribe(res => {
	// 		this.loadTintingMachinesPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "reports-table",
		tableClass: "table table-border ",
		tableName: this.tableName,
		tableRowIDInternalName: "id",
		tableColDef: [],
		// enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDataLength: true,
		enabledExcelDownload: true,
		downloadDataApiUrl: `${this.reportService.downloadLeadSummaryApiUrl(
								new LeadSummaryQuery({
									page: 1,
									pageSize: 2147483647, // Int32 max value
									sortBy: 'createdTime',
									isSortAscending: false,
									globalSearchValue: ''
								}))}`,
	};
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new LeadSummaryQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			// sortBy: queryObj.orderBy,
			sortBy: this.query.sortBy,
			// isSortAscending: queryObj.isOrderAsc,
			isSortAscending: this.query.isSortAscending,
			globalSearchValue: queryObj.searchVal
		});
		this.loadReportsPage();
	}
}
