import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { UniverseReachAnalysisKpiReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
    selector: 'app-universe-reach-analysis-report',
    templateUrl: './universe-reach-analysis-report.component.html',
    styleUrls: ['./universe-reach-analysis-report.component.css']
})
export class UniverseReachAnalysisReportComponent implements OnInit, OnDestroy {

	// data list
	query: UniverseReachAnalysisKpiReportQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// ptable settings
	enabledTotal: boolean = true;
	tableName: string = 'Universe Reach Analysis Report';
	// renameKeys: any = {'userId':'User Id'};
	renameKeys: any = {'outletNumber':'Outlet Number (Census)','directCovered':'Direct-Covered',
						'indirectCovered':'Indirect-Covered','uncovered':'Uncovered','directTarget':'Direct-Target',
						'indirectTarget':'Indirect-Target','directActual':'Direct Actual',
						'indirectActual':'Indirect (Ship to Party)', 'indirectManual':'Indirect (Manual)','covered':'Yet to Cover'};
	allTotalKeysOfNumberType: boolean = true;
	// totalKeys: any[] = ['totalCall'];
	totalKeys: any[] = [];

	ignoreKeys: any[] = ['billingAnalysisType','details','detailsBtnText'];

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
	getDownloadDataApiUrl = (query) => this.reportService.downloadUniverseReachAnalysis(query);
	getData = (query) => this.reportService.getUniverseReachAnalysis(query);

	searchConfiguration() {
		this.query = new UniverseReachAnalysisKpiReportQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: '',
			depot: '',
			//salesGroups: [],
			territories: [],
			year: null
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			//new SearchOptionDef({searchOption:EnumSearchOption.SalesGroup, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.FiscalYear, isRequired:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.salesGroups = queryObj.salesGroups;
		this.query.territories = queryObj.territories;
		this.query.year = queryObj.fiscalYear;
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
					// this.data.forEach((x) => {
					// 	x.detailsBtnText = "Details";
					// });
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
		this.ptableSettings.tableColDef = Object.keys(obj).filter(f => !this.ignoreKeys.includes(f)).map((key) => {
			return {
				headerName: this.commonService.insertSpaces(key), internalName: key,
				showTotal: (this.allTotalKeysOfNumberType
					? (typeof obj[key] === 'number')
					: this.totalKeys.includes(key)),
				type: typeof obj[key] === 'number' ? 'text' : null,
				displayType: typeof obj[key] === 'number' ? 'number-format-color' : null,
			} as colDef;
		});

		// this.ptableSettings.tableColDef.push(
		// 	{ headerName: 'Details', width: '10%', internalName: 'detailsBtnText', sort: false, type: "button",
		// 		onClick: 'true', innerBtnIcon: "" } as colDef);
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
		enabledCellClick: false,
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
	//#endregion
}
