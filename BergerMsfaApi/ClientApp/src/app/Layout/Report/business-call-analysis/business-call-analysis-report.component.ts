import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { forkJoin, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { finalize } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { BusinessCallAnalysisReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
    selector: 'app-business-call-analysis-report',
    templateUrl: './business-call-analysis-report.component.html',
    styleUrls: ['./business-call-analysis-report.component.css']
})
export class BusinessCallAnalysisReportComponent implements OnInit, OnDestroy {

	// data list
	query: BusinessCallAnalysisReportQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	
	// for filter
	fromDate: NgbDate;
	toDate: NgbDate;

	// ptable settings
	enabledTotal: boolean = false;
	tableName: string = 'Business Call Analysis Report';
	// renameKeys: any = {'userId':'User Id'};
	renameKeys: any = {};
	allTotalKeysOfNumberType: boolean = true;
	// totalKeys: any[] = ['totalCall'];
	totalKeys: any[] = [];

	// initial dropdown data
    depots: any[] = [];
    territories:any[]=[]
    zones: any[] = [];
	monthList: any[] = [];
	yearList: any[] = [];

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private reportService: ReportService,
		private modalService: NgbModal,
		private commonService: CommonService,
		private dynamicDropdownService: DynamicDropdownService) {
		// client side paggination
		// this.PAGE_SIZE = 2147483647; // Int32 max value
		// this.ptableSettings.pageSize = 10;
		// this.ptableSettings.enabledServerSitePaggination = false;
		// server side paggination
		this.PAGE_SIZE = commonService.PAGE_SIZE;
		this.ptableSettings.pageSize = this.PAGE_SIZE;
		this.ptableSettings.enabledServerSitePaggination = false;
	}

	ngOnInit() {
		this.searchConfiguration();
		this.populateDropdownDataList();
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	//#region need to change for another report
	getDownloadDataApiUrl = (query) => this.reportService.downloadBusinessCallAnalysis(query);
	getData = (query) => this.reportService.getBusinessCallAnalysis(query);
	
	searchConfiguration() {
		this.query = new BusinessCallAnalysisReportQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: '',
			depotId: '',
			territories: [],
			zones: [],
			fromDate: null,
			toDate: null
		});
	}
	
    populateDropdownDataList() {
        forkJoin([
            this.commonService.getDepotList(),
            this.commonService.getTerritoryList(),
            this.commonService.getZoneList(),
			this.commonService.getMonthList(),
            this.commonService.getYearList(),
        ]).subscribe(([plants, territories, zones, months, years]) => {
            this.depots = plants.data;
            this.territories = territories.data;
            this.zones = zones.data;
			this.monthList = months.data;
            this.yearList = years.data;
        }, (err) => { }, () => { });
    }
	
	//#endregion

	//#region no need to change for another report
	onSubmitSearch() {
		this.query.page = 1;
		this.query.fromDate = this.ngbDateToDate(this.fromDate);
		this.query.toDate = this.ngbDateToDate(this.toDate);
		this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
		this.loadReportsPage();
	}

	loadReportsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const reportsSubscription = this.getData(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.data = res.data;
					this.totalDataLength = res.data.length;
					this.totalFilterDataLength = res.data.length;
					this.ptableColDefGenerate();
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(reportsSubscription);
	}

	ptableColDefGenerate() {
		this.data = this.data.map(obj => { return this.commonService.renameKeys(obj, this.renameKeys)});
		const obj = this.data[0] || {};
		console.log(obj);
		this.ptableSettings.tableColDef = Object.keys(obj).map((key) => {
			return { headerName: this.commonService.insertSpaces(key), internalName: key, 
				showTotal: (this.allTotalKeysOfNumberType ? (typeof obj[key] === 'number') : this.totalKeys.includes(key)) } as colDef;
		});
		
	}

	public ptableSettings: IPTableSetting = {
		tableID: "reports-table",
		tableClass: "table table-border ",
		tableName: this.tableName,
		tableRowIDInternalName: "id",
		tableColDef: [],
		// enabledSearch: true,
		enabledSerialNo: true,
		pageSize: 10,
		enabledPagination: true,
		enabledDataLength: true,
		enabledTotal: this.enabledTotal,
		enabledExcelDownload: true,
		downloadDataApiUrl: `${this.getDownloadDataApiUrl(
								new QueryObject({
									page: 1,
									pageSize: 2147483647, // Int32 max value
									sortBy: 'createdTime',
									isSortAscending: false,
									globalSearchValue: ''
								}))}`,
	};
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query.page = queryObj.pageNo;
		this.query.pageSize = queryObj.pageSize;
		this.query.sortBy = queryObj.orderBy || this.query.sortBy;
		this.query.isSortAscending = queryObj.isOrderAsc || this.query.isSortAscending;
		this.query.globalSearchValue = queryObj.searchVal;
		this.loadReportsPage();
	}

	ngbDateToDate(date: NgbDate) : Date | null {
		return date && date.year && date.month && date.day ? 
				new Date(date.year,date.month-1,date.day) : 
				null;
	}
	//#endregion
}