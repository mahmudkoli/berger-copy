import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { OSOver90DaysTrendReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
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
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-os-over-90-days-trend-report',
  templateUrl: './os-over-90-days-trend-report.component.html',
  styleUrls: ['./os-over-90-days-trend-report.component.css'],
})
export class OSOver90DaysTrendReportComponent implements OnInit, OnDestroy {
  // data list
  query: OSOver90DaysTrendReportQuery;
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

  frommonth: any;
  fromyear: any;
  tomonth: any;
  toyear: any;

  // ptable settings
  enabledTotal: boolean = true;
  tableName: string = 'Active Summery Report';
  // renameKeys: any = {'userId':'// User Id //'};
  renameKeys: any = {};
  allTotalKeysOfNumberType: boolean = true;
  // totalKeys: any[] = ['totalCall'];
  totalKeys: any[] = [];

  // initial dropdown data
  employeeRoles: MapObject[] = EnumEmployeeRoleLabel.EmployeeRoles;
  users: any[] = [];
  creditControllArea: any[] = [];
  depots: any[] = [];
  salesGroups: any[] = [];
  territories: any[] = [];
  zones: any[] = [];
  dealerList: any[] = [];
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
    this.populateDropdownDataList();
    this.currentDateRangeSet();

    this.getDealerList();
    // of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
    // 	this.loadReportsPage();
    // });
  }
  private get _loggedUser() {
    return this.commonService.getUserInfoFromLocalStorage();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  //#region need to change for another report
  getDownloadDataApiUrl = (query) =>
    this.reportService.downloadOsOver90DaysTrend(query);
  getData = (query) => this.reportService.getOsOver90DaysTrend(query);

  private getDealerList() {
    if (this._loggedUser) {
      this.alertService.fnLoading(true);
      this.commonService
        .getDealerList(
          this._loggedUser.userCategory,
          this._loggedUser.userCategoryIds
        )
        .subscribe(
          (result: any) => {
            this.dealerList = result.data;
          },
          (err: any) => console.log(err)
        )
        .add(() => this.alertService.fnLoading(false));
    }
  }

  searchConfiguration() {
    this.query = new OSOver90DaysTrendReportQuery({
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
      this.commonService.getCreditControlAreaList(),
      this.commonService.getMonthList(),
      this.commonService.getYearList(),
    ]).subscribe(
      ([
        users,
        plants,
        areaGroups,
        territories,
        zones,
        creditZoneControll,
        month,
        year,
      ]) => {
        this.users = users.data;
        this.depots = plants.data;
        this.salesGroups = areaGroups.data;
        this.territories = territories.data;
        this.zones = zones.data;
        this.creditControllArea = creditZoneControll.data;
        this.monthList = month.data;
        this.yearList = year.data;
      },
      (err) => {},
      () => {}
    );
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

    this.MonthToDateConvert(
      this.frommonth,
      this.fromyear,
      this.tomonth,
      this.toyear
    );
    let res = this.monthDiff(this.query.fromDate, this.query.toDate);

    if (res == 2) {
      // this.query.fromDate = this.ngbDateToDate(this.fromDate);
      // this.query.toDate = this.ngbDateToDate(this.toDate);
      this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(
        this.query
      );
      this.loadReportsPage();
    } else {
      this.alertService.alert('From and To month difference must be 3 months.');
    }
  }

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
          this.data = res.data.items;
          this.totalDataLength = res.data.total;
          this.totalFilterDataLength = res.data.totalFilter;
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
      .filter((x) => x.internalName == 'change1' || x.internalName == 'change2')
      .forEach((element) => {
        element.headerName = 'Change';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'month1Value' ||
          x.internalName == 'month2Value' ||
          x.internalName == 'month3Value'
      )
      .forEach((element) => {
        let propertyName = element.internalName.replace('Value', 'Name');
        element.headerName = this.data[0][propertyName];
      });

    this.ptableSettings.tableColDef = this.ptableSettings.tableColDef.filter(
      (x) =>
        !(
          x.internalName == 'month1Name' ||
          x.internalName == 'month2Name' ||
          x.internalName == 'month3Name'
        )
    );
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

  // ngbDateToDate(date: NgbDate) : Date | null {
  // 	return date && date.year && date.month && date.day ?
  // 			new Date(date.year,date.month-1,date.day) :
  // 			null;
  // }

  private MonthToDateConvert(fromMonth, fromYear, toMonth, toYear) {
    var Lastday = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    //Set From Date from Month and Year
    // var frmdate = fromYear + "-" + fromMonth + "-" + 1+"T00:00:00";

    this.query.fromDate = new Date(fromYear, fromMonth - 1, 1);

    //Set To Date from Month and Year
    var day = Lastday[toMonth - 1];
    if (toMonth == 2 && this.leapYear(toYear)) {
      day = 29;
    }

    // var todate=toYear + "-" + toMonth + "-" + day+"T00:00:00";

    this.query.toDate = new Date(toYear, toMonth - 1, day);
  }

  private currentDateRangeSet() {
    var fd = new Date();
    var td = new Date();

    //set default from month
    fd.setMonth(fd.getMonth() - 2);
    this.frommonth = fd.getMonth() + 1;
    this.fromyear = fd.getFullYear();

    //set default to date
    td.setMonth(td.getMonth());
    this.tomonth = td.getMonth() + 1;
    this.toyear = td.getFullYear();
  }

  private leapYear(year) {
    return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
  }

  private monthDiff(d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth();
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
  }
  //#endregion
}
