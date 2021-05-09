import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { PremiumBrandsGrowthReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import {
  EnumEmployeeRole,
  EnumEmployeeRoleLabel,
} from 'src/app/Shared/Enums/employee-role';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import {
  colDef,
  IPTableServerQueryObj,
  IPTableSetting,
} from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-premium-brands-growth-report',
  templateUrl: './premium-brands-growth-report.component.html',
  styleUrls: ['./premium-brands-growth-report.component.css'],
})
export class PremiumBrandsGrowthReportComponent implements OnInit, OnDestroy {
  // data list
  query: PremiumBrandsGrowthReportQuery;
	searchOptionQuery: SearchOptionQuery;
  PAGE_SIZE: number;
  data: any[];
  totalDataLength: number = 0; // for server side paggination
  totalFilterDataLength: number = 0; // for server side paggination

  // ptable settings
  enabledTotal: boolean = true;
  tableName: string = 'Premium Brands Growth Report';
  // renameKeys: any = {'userId':'// User Id //'};
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
    private dynamicDropdownService: DynamicDropdownService
  ) {
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
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  //#region need to change for another report
  getDownloadDataApiUrl = (query) =>
    this.reportService.downloadPremiumBrandsGrowth(query);
  getData = (query) => this.reportService.getPremiumBrandsGrowth(query);

  searchConfiguration() {
    this.query = new PremiumBrandsGrowthReportQuery({
      page: 1,
      pageSize: this.PAGE_SIZE,
      sortBy: 'createdTime',
      isSortAscending: false,
      globalSearchValue: '',
      depot: '',
      salesGroups: [],
      territories: [],
      zones: [],
      fromMonth: null,
      fromYear: null,
      toMonth: null,
      toYear: null,
    });
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
    hasMonthDifference: true,
    monthDifferenceCount: 3,
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.SalesGroup, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Zone, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.FromMonth, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.FromYear, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.ToMonth, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.ToYear, isRequired:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.salesGroups = queryObj.salesGroups;
		this.query.territories = queryObj.territories;
		this.query.zones = queryObj.zones;
		this.query.fromMonth = queryObj.fromMonth;
		this.query.fromYear = queryObj.fromYear;
		this.query.toMonth = queryObj.toMonth;
		this.query.toYear = queryObj.toYear;
		this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
		this.loadReportsPage();
	}
  //#endregion

  //#region no need to change for another report
  loadReportsPage() {
    // this.searchConfiguration();
    this.alertService.fnLoading(true);
    const reportsSubscription = this.getData(this.query)
      .pipe(
        finalize(() => {
          this.alertService.fnLoading(false);
        })
      )
      .subscribe(
        (res) => {
          this.data = res.data;
          this.totalDataLength = res.data.length;
          this.totalFilterDataLength = res.data.length;
          this.ptableColDefGenerate();
        },
        (error) => {
          console.log(error);
        }
      );
    this.subscriptions.push(reportsSubscription);
  }

  ptableColDefGenerate() {
    this.data = this.data.map((obj) => {
      return this.commonService.renameKeys(obj, this.renameKeys);
    });
    const obj = this.data[0] || {};
    this.ptableSettings.tableColDef = Object.keys(obj).map((key) => {
      return {
        headerName: this.commonService.insertSpaces(key),
        internalName: key,
        showTotal: this.allTotalKeysOfNumberType
          ? typeof obj[key] === 'number'
          : this.totalKeys.includes(key),
      } as colDef;
    });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'totalTarget' || x.internalName == 'totalActual'
      )
      .forEach((element) => {
        element.headerName =
          element.internalName == 'totalActual' ? 'Total CY' : 'Total LY';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'firstMonthTargetAmount' ||
          x.internalName == 'secondMonthTargetAmount' ||
          x.internalName == 'thirdMonthTargetAmount' ||
          x.internalName == 'firstMonthActualAmount' ||
          x.internalName == 'secondMonthActualAmount' ||
          x.internalName == 'thirdMonthActualAmount'
      )
      .forEach((element) => {
        let propertyName = element.internalName.replace('Amount', 'Name');
        element.headerName = this.data[0][propertyName];
      });

    this.ptableSettings.tableColDef = this.ptableSettings.tableColDef.filter(
      (x) =>
        !(
          x.internalName == 'firstMonthTargetName' ||
          x.internalName == 'secondMonthTargetName' ||
          x.internalName == 'thirdMonthTargetName' ||
          x.internalName == 'firstMonthActualName' ||
          x.internalName == 'secondMonthActualName' ||
          x.internalName == 'thirdMonthActualName'
        )
    );

    let ach = this.ptableSettings.tableColDef.find(
      (x) => x.internalName == 'achivementOrGrowth'
    );
    if (ach) {
      ach.headerName = 'Growth%';
    }
  }

  public ptableSettings: IPTableSetting = {
    tableID: 'reports-table',
    tableClass: 'table table-border ',
    tableName: this.tableName,
    tableRowIDInternalName: 'id',
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
        globalSearchValue: '',
      })
    )}`,
  };

  serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
    this.query.page = queryObj.pageNo;
    this.query.pageSize = queryObj.pageSize;
    this.query.sortBy = queryObj.orderBy || this.query.sortBy;
    this.query.isSortAscending =
      queryObj.isOrderAsc || this.query.isSortAscending;
    this.query.globalSearchValue = queryObj.searchVal;
    this.loadReportsPage();
  }
  //#endregion
}
