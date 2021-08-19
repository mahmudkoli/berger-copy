import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ColorBankProductivityKpiReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import {
  colDef,
  IPTableServerQueryObj,
  IPTableSetting,
} from 'src/app/Shared/Modules/p-table';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings,
} from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';

@Component({
  selector: 'app-color-bank-productivity',
  templateUrl: './color-bank-productivity.component.html',
  styleUrls: ['./color-bank-productivity.component.css'],
})
export class ColorBankProductivityComponent implements OnInit, OnDestroy {
  query: ColorBankProductivityKpiReportQuery;
  searchOptionQuery: SearchOptionQuery;
  PAGE_SIZE: number;
  data: any[];
  totalDataLength: number = 0; // for server side paggination
  totalFilterDataLength: number = 0; // for server side paggination

  // ptable settings
  enabledTotal: boolean = false;
  tableName: string = 'Color Bank Productivity';
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
    this.ptableSettings.pageSize = 10;
    this.ptableSettings.defaultPaggingSize = 10;
  }

  ngOnInit() {
    this.searchConfiguration();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  searchConfiguration() {
    this.query = new ColorBankProductivityKpiReportQuery({
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
      new SearchOptionDef({ searchOption: EnumSearchOption.SalesGroup }),
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
    conditionalRowStyles: [
      { columnName: 'territory', columnValues: ['Total'] },
    ],

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

  getData = (query) => this.reportService.getColorBankProductivity(query);

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
          displayType:
            typeof obj[key] === 'number'
              ? 'number-format-color-fraction'
              : null,
        } as colDef;
      });
  }
}
