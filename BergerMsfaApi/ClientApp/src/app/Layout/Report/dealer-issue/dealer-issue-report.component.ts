import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { forkJoin, of, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { DealerIssueReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { EnumEmployeeRole, EnumEmployeeRoleLabel } from 'src/app/Shared/Enums/employee-role';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
    selector: 'dealer-issue-report',
    templateUrl: './dealer-issue-report.component.html',
    styleUrls: ['./dealer-issue-report.component.css']
})
export class DealerIssueReportComponent implements OnInit, OnDestroy {

	// data list
	query: DealerIssueReportQuery;
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
	tableName: string = 'Dealer Issue Report';
	// renameKeys: any = {'userId':'// User Id //'};
	renameKeys: any = {
		'pcMaterial' : 'Material',
		'pcMaterialGroup' : 'Material Group',
		'pcQuantity' : 'Quantity',
		'pcBatchNumber' : 'Batch Number',
		'pcComments' : 'Comments',
		'pcPriority' : 'Priority',
		'posComments' : 'pcomment',
		'posPriority' : 'ppriority',
		'shadeComments' : 'scomments',
		'shadePriority' : 'spriority',
		'shopSignComments' : 'sscomments',
		'shopSignPriority' : 'sspriority',
		'deliveryComments' : 'dcomments',
		'deliveryPriority' : 'dpriority',
		'damageMaterial' : 'Materials',
		'damageMaterialGroup' : 'Materials Group',
		'damageQuantity' : 'dquantity',
		'damageComments' : 'damComments',
		'damagecPriority' : 'damPriority',
		'cbmStatus' : 'Status',
		'cbmMaintatinanceFrequency' : 'Maintatinance Frequency',
		'cbmRemarks' : 'Remarks for Irregular',
		'cbmPriority' : 'cbmPriority',
		'othersComment' : 'ocomment',
		'othersriority' : 'opriority'
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
	dealerList: any[] = [];

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
	getDownloadDataApiUrl = (query) => this.reportService.downloadDealerIssue(query);
	getData = (query) => this.reportService.getDealerIssue(query);
	
	private getDealerList() {
        if (this._loggedUser) {
            this.alertService.fnLoading(true);
            this.commonService.getDealerList(this._loggedUser.userCategory, this._loggedUser.userCategoryIds).subscribe(
                (result: any) => {
                    this.dealerList = result.data;
                },
                (err: any) => console.log(err)

            ).add(() => this.alertService.fnLoading(false));
        }

    }

	searchConfiguration() {
		this.query = new DealerIssueReportQuery({
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
			(x) => x.internalName == 'Material' || x.internalName == 'Material Group' || x.internalName == 'Quantity' || x.internalName == 'Batch Number' || x.internalName == 'Comments' || x.internalName == 'Priority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Product Complaint';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'pcomment' || x.internalName == 'ppriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'POS Material Short';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'scomments' || x.internalName == 'spriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Shade Card';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'sscomments' || x.internalName == 'sspriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Shop Sign Complain';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'Status' || x.internalName == 'Maintatinance Frequency' || x.internalName == 'Remarks for Irregular' || x.internalName == 'cbmPriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Color Bank Maintainance';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'Materials' || x.internalName == 'Materials Group' || x.internalName == 'dquantity' || x.internalName == 'damComments' || x.internalName == 'damPriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Damage Product';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'ocomment' || x.internalName == 'opriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Others';
		});

		this.ptableSettings.tableColDef
		.filter(
			(x) => x.internalName == 'dcomments' || x.internalName == 'dpriority'
		)
		.forEach((x) => {
			x.parentHeaderName = 'Delivery Issue';
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
