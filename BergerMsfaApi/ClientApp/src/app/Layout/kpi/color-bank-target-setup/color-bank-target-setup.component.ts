import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { StrikeRateKpiReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings,
} from 'src/app/Shared/Modules/search-option';

@Component({
  selector: 'app-color-bank-target-setup',
  templateUrl: './color-bank-target-setup.component.html',
  styleUrls: ['./color-bank-target-setup.component.css'],
})
export class ColorBankTargetSetupComponent implements OnInit {
  // data list
  query: StrikeRateKpiReportQuery;
  searchOptionQuery: SearchOptionQuery;
  PAGE_SIZE: number;
  data: any[];
  totalDataLength: number = 0; // for server side paggination
  totalFilterDataLength: number = 0; // for server side paggination

  // ptable settings
  enabledTotal: boolean = false;
  tableName: string = 'Strike rate on business call Report';
  // renameKeys: any = {'userId':'User Id'};
  renameKeys: any = {};
  allTotalKeysOfNumberType: boolean = true;
  // totalKeys: any[] = ['totalCall'];
  totalKeys: any[] = [];

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor() {}

  ngOnInit() {
    this.searchConfiguration();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  searchConfiguration() {
    this.query = new StrikeRateKpiReportQuery({
      page: 1,
      pageSize: this.PAGE_SIZE,
      sortBy: 'createdTime',
      isSortAscending: false,
      globalSearchValue: '',
      depot: '',
      salesGroups: [],
      territories: [],
      zones: [],
      month: null,
      year: null,
      reportType: null,
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
      new SearchOptionDef({
        searchOption: EnumSearchOption.Year,
        isRequired: true,
      }),
    ],
  });

  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    console.log('Search option query callback: ', queryObj);
    this.query.depot = queryObj.depot;
    this.query.salesGroups = queryObj.salesGroups;
    this.query.territories = queryObj.territories;
    this.query.zones = queryObj.zones;
    this.query.month = queryObj.month;
    this.query.year = queryObj.year;
    this.query.reportType = queryObj.customerClassificationType;

    // this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(
    //   this.query
    // );
    // this.loadReportsPage();
  }
}
