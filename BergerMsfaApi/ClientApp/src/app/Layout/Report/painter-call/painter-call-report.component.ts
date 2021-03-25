import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { PaintersCallReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
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
  selector: 'app-painter-call-report',
  templateUrl: './painter-call-report.component.html',
  styleUrls: ['./painter-call-report.component.css'],
})
export class PainterCallReportComponent implements OnInit, OnDestroy {
  // data list
  query: PaintersCallReportQuery;
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
  tableName: string = 'Painter Call Report';
  // renameKeys: any = {'userId':'// User Id //'};
  // renameKeys: any = [
  // 	{'appInstalledStatus': 'Shamparka APP Installed Status'},
  // 	{'appNotInstalledReason': 'Shamparka App Not Installed "Reason"'},
  // ];
  renameKeys: any = {};
  allTotalKeysOfNumberType: boolean = true;
  // totalKeys: any[] = ['totalCall'];
  totalKeys: any[] = [];

  // initial dropdown data
  employeeRoles: MapObject[] = EnumEmployeeRoleLabel.EmployeeRoles;
  users: any[] = [];
  depots: any[] = [];
  salesGroups: any[] = [];
  territories: any[] = [];
  zones: any[] = [];
  painters: any[] = [];
  painterTypes: any[] = [];

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
    // of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
    // 	this.loadReportsPage();
    // });
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  //#region need to change for another report
  getDownloadDataApiUrl = (query) =>
    this.reportService.downloadPainterCall(query);
  getData = (query) => this.reportService.getPainterCall(query);

  searchConfiguration() {
    this.query = new PaintersCallReportQuery({
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
      this.commonService.getPainterList(),
      this.dynamicDropdownService.GetDropdownByTypeCd(
        EnumDynamicTypeCode.Painter
      ),
    ]).subscribe(
      ([
        users,
        plants,
        areaGroups,
        territories,
        zones,
        painters,
        painterTypes,
      ]) => {
        this.users = users.data;
        this.depots = plants.data;
        this.salesGroups = areaGroups.data;
        this.territories = territories.data;
        this.zones = zones.data;
        this.painters = painters.data;
        this.painterTypes = painterTypes.data;
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
    this.query.fromDate = this.ngbDateToDate(this.fromDate);
    this.query.toDate = this.ngbDateToDate(this.toDate);
    this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(
      this.query
    );
    this.loadReportsPage();
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
          console.log('res.data', res.data);
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
      .filter(
        (x) => x.internalName == 'bpblMtdValue' || x.internalName == 'bpblCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'BPBL';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) => x.internalName == 'duluxMtdValue' || x.internalName == 'duluxCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Dulux';
      });
    
    this.ptableSettings.tableColDef
      .filter(
        (x) => x.internalName == 'moonstarMtdValue' || x.internalName == 'moonstarCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Moonstar';
      });  

    this.ptableSettings.tableColDef
      .filter(
        (x) => x.internalName == 'apMtdValue' || x.internalName == 'apCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'AP';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'nerolacMtdValue' ||
          x.internalName == 'nerolacCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Nerolac';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'nipponMtdValue' || x.internalName == 'nipponCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Nippon';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'eliteMtdValue' || x.internalName == 'eliteCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Elite';
      });

    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'othersMtdValue' || x.internalName == 'othersCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Others';
      });
    this.ptableSettings.tableColDef
      .filter(
        (x) =>
          x.internalName == 'totalMtdValue' || x.internalName == 'totalCount'
      )
      .forEach((x) => {
        x.parentHeaderName = 'Total';
      });
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
    console.log('server site : ', queryObj);
    this.query.page = queryObj.pageNo;
    this.query.pageSize = queryObj.pageSize;
    this.query.sortBy = queryObj.orderBy || this.query.sortBy;
    this.query.isSortAscending =
      queryObj.isOrderAsc || this.query.isSortAscending;
    this.query.globalSearchValue = queryObj.searchVal;
    this.loadReportsPage();
  }

  ngbDateToDate(date: NgbDate): Date | null {
    return date && date.year && date.month && date.day
      ? new Date(date.year, date.month - 1, date.day)
      : null;
  }
  //#endregion
}
