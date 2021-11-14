import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { finalize } from 'rxjs/operators';
import { ColorBankInstallationPlanVsActualKpiReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import {
  IPTableServerQueryObj,
  IPTableSetting
} from 'src/app/Shared/Modules/p-table';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings
} from 'src/app/Shared/Modules/search-option';
import { AlertService } from './../../../Shared/Modules/alert/alert.service';
import { colDef } from './../../../Shared/Modules/p-table/p-table';
import { CommonService } from './../../../Shared/Services/Common/common.service';
import { ReportService } from './../../../Shared/Services/Report/ReportService';

@Component({
  selector: 'app-color-bank-installation-plan-vs-actual',
  templateUrl: './color-bank-installation-plan-vs-actual.component.html',
  styleUrls: ['./color-bank-installation-plan-vs-actual.component.css'],
})
export class ColorBankInstallationPlanVsActualComponent
  implements OnInit, OnDestroy
{
  query: ColorBankInstallationPlanVsActualKpiReportQuery;
  searchOptionQuery: SearchOptionQuery;
  PAGE_SIZE: number;
  data: any[];
  totalDataLength: number = 0; // for server side paggination
  totalFilterDataLength: number = 0; // for server side paggination

  // ptable settings
  enabledTotal: boolean = false;
  tableName: string = 'Color Bank Installation Plan Vs Actual';
  // renameKeys: any = {'userId':'User Id'};
  renameKeys: any = {};
  allTotalKeysOfNumberType: boolean = true;
  // totalKeys: any[] = ['totalCall'];
  totalKeys: any[] = [];

  ignoreKeys: any[] = [];

  private subscriptions: Subscription[] = [];

  constructor(
    private reportService: ReportService,
    private alertService: AlertService,
    private commonService: CommonService
  ) {
    this.ptableSettings.pageSize = 20;
    this.ptableSettings.defaultPaggingSize = 20;
  }

  ngOnInit() {
    this.searchConfiguration();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  searchConfiguration() {
    this.query = new ColorBankInstallationPlanVsActualKpiReportQuery({
      depot: '',
      territories: [],
    });
    this.searchOptionQuery = new SearchOptionQuery();
    this.searchOptionQuery.clear();
  }

  searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
    searchOptionDef: [
      new SearchOptionDef({
        searchOption: EnumSearchOption.Depot,
        isRequired: true,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.Territory,
        isRequired: true,
      }),
      //new SearchOptionDef({ searchOption: EnumSearchOption.SalesGroup }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.FiscalYear,
        isRequired: true,
      }),
    ],
  });

  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    this.query.depot = queryObj.depot;
    this.query.territories = queryObj.territories;
    this.query.salesGroups = queryObj.salesGroups;
    this.query.year = queryObj.fiscalYear;

    // this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
    this.loadReportsPage();
  }

  public ptableSettings: IPTableSetting = {
    tableID: 'reports-table',
    tableClass: 'table table-border ',
    tableName: this.tableName,
    tableRowIDInternalName: 'id',
    tableColDef: [],
    // enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledDataLength: true,
    enabledTotal: this.enabledTotal,
    enabledCellClick: true,
    enabledExcelDownload: true,
    enabledConditionalRowStyles: true,
    conditionalRowStyles: [{ columnName: 'month', columnValues: ['Total'] }],

    // downloadDataApiUrl: `${this.getDownloadDataApiUrl(
    //             new QueryObject({
    //               page: 1,
    //               pageSize: 2147483647, // Int32 max value
    //               sortBy: 'createdTime',
    //               isSortAscending: false,
    //               globalSearchValue: ''
    //             }))}`,
  };

  serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
    this.loadReportsPage();
  }

  loadReportsPage() {
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

  getDownloadDataApiUrl = (query) =>
    this.reportService.downloadFinancialCollectionPlan(query);
  getData = (query) =>
    this.reportService.getColorBankInstallationPlanVsActual(query);

  ptableColDefGenerate() {
    this.data = this.data.map((obj) => {
      return this.commonService.renameKeys(obj, this.renameKeys);
    });
    const obj = this.data[0] || {};
    this.ptableSettings.tableColDef = Object.keys(obj)
      .filter((f) => !this.ignoreKeys.includes(f))
      .map((key) => {
        return {
          headerName: this.commonService.insertSpaces(key),
          internalName: key,
          showTotal: this.allTotalKeysOfNumberType
            ? typeof obj[key] === 'number'
            : this.totalKeys.includes(key),
            type: typeof obj[key] === 'number' ? 'text' : null,
          displayType:
            typeof obj[key] === 'number'
              ? 'number-format-color-fraction'
              : null,
        } as colDef;
      });
  }
}
