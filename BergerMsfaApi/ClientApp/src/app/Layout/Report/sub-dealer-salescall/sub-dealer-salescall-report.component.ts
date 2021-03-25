import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { forkJoin, of, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { SubDealerSalesCallReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { EnumEmployeeRole, EnumEmployeeRoleLabel } from 'src/app/Shared/Enums/employee-role';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
    selector: 'sub-dealer-salescall-report',
    templateUrl: './sub-dealer-salescall-report.component.html',
    styleUrls: ['./sub-dealer-salescall-report.component.css']
})
export class SubDealerSalescallReportComponent implements OnInit, OnDestroy {

	// data list
	query: SubDealerSalesCallReportQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	
	// for filter
	fromDate: NgbDate;
	toDate: NgbDate;
	isSalesGroupFieldShow: boolean = false;
	isTerritoryFieldShow: boolean = false;
	isZoneFieldShow: boolean = false;

	// ptable settings
	enabledTotal: boolean = true;
	tableName: string = 'Sub Dealer Sales Call Report';
	// renameKeys: any = {'userId':'// User Id //'};
	renameKeys: any = {
		'ssStatus' : 'Status',
		'ssReasonForPourOrAverage' : 'Reason for poor or Average',
		'osActivity' : 'Activity',
		'painterInfluecePercent' : 'Influence %',
		'csRemarks' : 'Remarks',
		'pdmRemarks' : 'Remark',
		'dealerSatisfactionStatus' : 'dStatus'
	};
	allTotalKeysOfNumberType: boolean = true;
	// totalKeys: any[] = ['totalCall'];
	totalKeys: any[] = [];

	// initial dropdown data
	employeeRoles: MapObject[] = EnumEmployeeRoleLabel.EmployeeRoles;
    users: any[] = [];
    depots: any[] = [];
    salesGroups: any[] = [];
    territories:any[]=[]
    zones: any[] = [];
	subDealerList: any[] = [];

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
		this.populateDropdownDataList();
		this.getDealerList();
		// of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
		// 	this.loadReportsPage();
		// });
	}
	private get _loggedUser() { return this.commonService.getUserInfoFromLocalStorage(); }

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}
	
	//#region need to change for another report
	getDownloadDataApiUrl = (query) => this.reportService.downloadSubDealerSalesCall(query);
	getData = (query) => this.reportService.getSubDealerSalesCall(query);
	
	private getDealerList() {
        if (this._loggedUser) {
            this.alertService.fnLoading(true);
            this.commonService.getDealerList(this._loggedUser.userCategory, this._loggedUser.userCategoryIds).subscribe(
                (result: any) => {
                    this.subDealerList = result.data;
                },
                (err: any) => console.log(err)

            ).add(() => this.alertService.fnLoading(false));
        }

    }

	searchConfiguration() {
		this.query = new SubDealerSalesCallReportQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: '',
			depotId: '',
			employeeRole: null,
			salesGroups: [],
			territories: [],
			zones: [],
			userId: null,
			fromDate: null,
			toDate: null,
		});
	}
	
    populateDropdownDataList() {
        forkJoin([
            this.commonService.getUserInfoListByLoggedInManager(),
            this.commonService.getDepotList(),
            this.commonService.getSaleGroupList(),
            this.commonService.getTerritoryList(),
            this.commonService.getZoneList(),
			this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Payment),
        ]).subscribe(([users, plants, areaGroups, territories, zones]) => {
            this.users = users.data;
            this.depots = plants.data;
            this.salesGroups = areaGroups.data;
            this.territories = territories.data;
            this.zones = zones.data;
        }, (err) => { }, () => { });
    }

	onEmployeeRoleChange(event) {
		this.query.salesGroups = [];
		this.query.territories = [];
		this.query.zones = [];

		switch (event) {
			case EnumEmployeeRole.DIC:
			case EnumEmployeeRole.BIC:
			case EnumEmployeeRole.AM:
				this.isSalesGroupFieldShow = true;
				this.isTerritoryFieldShow = true;
				this.isZoneFieldShow = true;
				break;
			case EnumEmployeeRole.TM_TO:
				this.isSalesGroupFieldShow = false;
				this.isTerritoryFieldShow = true;
				this.isZoneFieldShow = true;
				break;
				case EnumEmployeeRole.ZO:
					this.isSalesGroupFieldShow = false;
					this.isTerritoryFieldShow = false;
					this.isZoneFieldShow = true;
					break;
			default:
				this.isSalesGroupFieldShow = false;
				this.isTerritoryFieldShow = false;
				this.isZoneFieldShow = false;
				break;
		}
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
			(x) => x.internalName == 'osStatus' || x.internalName == 'Activity'
		)
		.forEach((x) => {
			x.parentHeaderName = 'OS Communication';
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
			(x) => x.internalName == 'productKnoledge' || x.internalName == 'salesTechniques' || x.internalName == 'merchendisingImprovement'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Shop Manage/Shop Boy';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'painterInfluence' || x.internalName == 'Influence %'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Painter';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'bergerAvrgMonthlySales' || x.internalName == 'bergerActualMtdSales'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Berger Sales';
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

	ngbDateToDate(date: NgbDate) : Date | null {
		return date && date.year && date.month && date.day ? 
				new Date(date.year,date.month-1,date.day) : 
				null;
	}
	//#endregion
}
