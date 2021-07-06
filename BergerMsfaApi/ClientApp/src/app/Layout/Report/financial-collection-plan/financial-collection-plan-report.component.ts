import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { forkJoin, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbDate, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { finalize } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { BillingAnalysisKpiReportQuery, BusinessCallAnalysisReportQuery, CollectionPlanKpiReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { ModalBillingAnalysisDetailsComponent } from '../modal-billing-analysis-details/modal-billing-analysis-details.component';

@Component({
    selector: 'app-financial-collection-plan-report',
    templateUrl: './financial-collection-plan-report.component.html',
    styleUrls: ['./financial-collection-plan-report.component.css']
})
export class FinancialCollectionPlanReportComponent implements OnInit, OnDestroy {

	// data list
	query: CollectionPlanKpiReportQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	
	// ptable settings
	enabledTotal: boolean = false;
	tableName: string = 'Collection Plan Report';
	// renameKeys: any = {'userId':'User Id'};
	renameKeys: any = {};
	allTotalKeysOfNumberType: boolean = true;
	// totalKeys: any[] = ['totalCall'];
	totalKeys: any[] = [];

	ignoreKeys: any[] = [];

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
	getDownloadDataApiUrl = (query) => this.reportService.downloadFinancialCollectionPlan(query);
	getData = (query) => this.reportService.getFinancialCollectionPlan(query);
	
	searchConfiguration() {
		this.query = new CollectionPlanKpiReportQuery({
			depot: '',
			territory: '',
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequired:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.territory = queryObj.territories[0];
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
		this.ptableSettings.tableColDef = Object.keys(obj).filter(f => !this.ignoreKeys.includes(f)).map((key) => {
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
		enabledCellClick: true,
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
		this.loadReportsPage();
	}
	//#endregion
}