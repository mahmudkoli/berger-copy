import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { forkJoin, of, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { DealerSalesCallReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { EnumEmployeeRole, EnumEmployeeRoleLabel } from 'src/app/Shared/Enums/employee-role';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';

@Component({
    selector: 'addhoc-dealer-salescall-report',
    templateUrl: './addhoc-dealer-salescall-report.component.html',
    styleUrls: ['./addhoc-dealer-salescall-report.component.css']
})
export class AddhocDealerSalescallReportComponent implements OnInit, OnDestroy {

	// data list
	query: DealerSalesCallReportQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// ptable settings
	enabledTotal: boolean = true;
	tableName: string = 'Addhoc Dealer Sales Call Report';
	// renameKeys: any = {'userId':'// User Id //'};
	renameKeys: any = {
		'ssStatus' : 'Status',
		'ssReasonForPourOrAverage' : 'Reason for poor or Average',
		'sdInfluecePercent' : 'Influence %',
		'painterInfluecePercent' : 'Influenc %',
		'csRemarks' : 'Remarks',
		'pdmRemarks' : 'Remark',
		'dealerSatisfactionStatus' : 'dStatus'
	};
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
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}
	
	//#region need to change for another report
	getDownloadDataApiUrl = (query) => this.reportService.downloadAddhocDealerSalesCall(query);
	getData = (query) => this.reportService.getAddhocDealerSalesCall(query);

	searchConfiguration() {
		this.query = new DealerSalesCallReportQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: '',
			depot: '',
			salesGroups: [],
			territories: [],
			zones: [],
			userId: null,
			dealerId: null,
			fromDate: null,
			toDate: null,
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		isDealerShow: true,
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.SalesGroup, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Zone, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.FromDate, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.ToDate, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.UserId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.DealerId, isRequired:false}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.salesGroups = queryObj.salesGroups;
		this.query.territories = queryObj.territories;
		this.query.zones = queryObj.zones;
		this.query.fromDate = queryObj.fromDate;
		this.query.toDate = queryObj.toDate;
		this.query.userId = queryObj.userId;
		this.query.dealerId = queryObj.dealerId;
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
					this.data = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
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
		this.ptableSettings.tableColDef = Object.keys(obj).map((key) => {
			return { headerName: this.commonService.insertSpaces(key), internalName: key, 
				showTotal: (this.allTotalKeysOfNumberType ? (typeof obj[key] === 'number') : this.totalKeys.includes(key)) } as colDef;
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'Status' || x.internalName == 'Reason for poor or Average'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Secondary Sales';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'uspCommunication' || x.internalName == 'productLiftingStatus' || x.internalName == 'reasonForNotLifting'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Premium Product Activity';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'cbMachineStatus' || x.internalName == 'cbProductivity'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Color Bank';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'subDealerInfluence' || x.internalName == 'Influence %'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Sub-Dealer Management';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'painterInfluence' || x.internalName == 'Influenc %'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Painter';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'productKnoledge' || x.internalName == 'salesTechniques' || x.internalName == 'merchendisingImprovement'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Shop Manage/Shop Boy';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'competitionService' || x.internalName == 'Remarks' || x.internalName == 'productDisplayAndMerchendizingStatus' || x.internalName == 'Remark' || x.internalName == 'productDisplayAndMerchendizingImage' || x.internalName == 'schemeModality' || x.internalName == 'schemeModalityImage' || x.internalName == 'shopBoy'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Competition Information';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'apAvrgMonthlySales' || x.internalName == 'apActualMtdSales' || x.internalName == 'nerolacAvrgMonthlySales' || x.internalName == 'nerolacActualMtdSales' || x.internalName == 'nipponAvrgMonthlySales' || x.internalName == 'nipponActualMtdSales' || x.internalName == 'duluxAvrgMonthlySales' || x.internalName == 'duluxActualMtdSales'  || x.internalName == 'jotunAvrgMonthlySales' || x.internalName == 'jotunActualMtdSales' || x.internalName == 'moonstarAvrgMonthlySales' || x.internalName == 'moonstarActualMtdSales' || x.internalName == 'eliteAvrgMonthlySales' || x.internalName == 'eliteActualMtdSales' || x.internalName == 'alkarimAvrgMonthlySales' || x.internalName == 'alkarimActualMtdSales' || x.internalName == 'othersAvrgMonthlySales' || x.internalName == 'othersActualMtdSales' || x.internalName == 'totalAvrgMonthlySales' || x.internalName == 'totalActualMtdSales'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Competition Sales';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'dStatus' || x.internalName == 'dealerDissatisfactionReason'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Dealer Satisfaction';
		});

		// Show Image
		var columName = this.ptableSettings.tableColDef.filter(x => 
						x.internalName == 'productDisplayAndMerchendizingImage' ||
						x.internalName == 'schemeModalityImage'
			);
		if(columName.length > 0){
			columName[0].type = 'image';
			columName[1].type = 'image';
		}

		// console.log(this.ptableSettings.tableColDef);
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