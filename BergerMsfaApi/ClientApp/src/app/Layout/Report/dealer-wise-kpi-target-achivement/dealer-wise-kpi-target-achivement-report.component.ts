import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { DealerWiseTargetAchivementQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
    selector: 'app-dealer-wise-kpi-target-achivement-report',
    templateUrl: './dealer-wise-kpi-target-achivement-report.component.html',
    styleUrls: ['./dealer-wise-kpi-target-achivement-report.component.css']
})
export class DealerWiseKpiTargetAchivementReportComponent implements OnInit, OnDestroy {

	// data list
	query: DealerWiseTargetAchivementQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// ptable settings
	enabledTotal: boolean = false;
	tableName: string = 'Dealer Wise Target Achievement Report';
	// renameKeys: any = {'userId':'User Id'};
	renameKeys: any = {};
	allTotalKeysOfNumberType: boolean = true;
	// totalKeys: any[] = ['totalCall'];
	totalKeys: any[] = [];

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
		this.PAGE_SIZE = 2147483647; // Int32 max value
		this.ptableSettings.pageSize = 10;
		this.ptableSettings.enabledServerSitePaggination = false;
		// server side paggination
		// this.PAGE_SIZE = commonService.PAGE_SIZE;
		// this.ptableSettings.pageSize = this.PAGE_SIZE;
		// this.ptableSettings.enabledServerSitePaggination = true;
	}

	ngOnInit() {
		this.searchConfiguration();
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	//#region need to change for another report
	getDownloadDataApiUrl = (query) => this.reportService.DownloadDealerWiseTargetAchivement(query);
	getData = (query) => this.reportService.getDealerWiseTargetAchivement(query);

	searchConfiguration() {
		this.query = new DealerWiseTargetAchivementQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: '',
			depot: '',
			//salesGroups: [],
			territories: [],
			fromDate: null,
			toDate: null,
			customerNo: null,
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			//new SearchOptionDef({searchOption:EnumSearchOption.SalesGroup, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.FromDate, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.ToDate, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.DealerId, isRequired:false}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.salesGroups = queryObj.salesGroups;
		this.query.territories = queryObj.territories;
		this.query.fromDate = queryObj.fromDate;
		this.query.toDate = queryObj.toDate;
		this.query.customerNo = queryObj.customerNo;
		this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
		this.loadReportsPage();
	}
	//#endregion

	//#region no need to change for another report
	loadReportsPage() {
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
			return {
				headerName: this.commonService.insertSpaces(key), internalName: key,
				showTotal: (this.allTotalKeysOfNumberType ? (typeof obj[key] === 'number') : this.totalKeys.includes(key)),
				type: typeof obj[key] === 'number' ? 'text' : null,
				displayType: typeof obj[key] === 'number' ? 'number-format-color-fraction' : null,
			} as colDef;
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
		// pageSize: 10,
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
		enabledConditionalRowStyles:true,
		conditionalRowStyles: [
			{columnName:'territory',columnValues:['Total']}
		],
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
	//#endregion
}
