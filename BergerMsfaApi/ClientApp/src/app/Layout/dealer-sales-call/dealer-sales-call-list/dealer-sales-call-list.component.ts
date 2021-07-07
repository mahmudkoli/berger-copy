import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import {
  DealerSalesCall,
  DealerSalesCallQuery,
} from 'src/app/Shared/Entity/DealerSalesCall/dealer-sales-call';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import {
  IPTableServerQueryObj,
  IPTableSetting,
} from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings,
} from './../../../Shared/Modules/search-option/search-option';

@Component({
  selector: 'app-dealer-sales-call-list',
  templateUrl: './dealer-sales-call-list.component.html',
  styleUrls: ['./dealer-sales-call-list.component.css'],
})
export class DealerSalesCallListComponent implements OnInit, OnDestroy {
  searchOptionQuery: SearchOptionQuery;
  query: DealerSalesCallQuery;
  PAGE_SIZE: number;
  dealerSalesCalls: DealerSalesCall[];
  isSearchClick = false;
  totalDataLength: number = 0; // for server side paggination
  totalFilterDataLength: number = 0; // for server side paggination
  searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
    isDealerSubDealerOptionShow: true,
    searchOptionDef: [
      new SearchOptionDef({
        searchOption: EnumSearchOption.Depot,
        isRequired: true,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.SalesGroup,
        isRequired: false,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.Territory,
        isRequired: false,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.Zone,
        isRequired: false,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.DealerId,
        isRequired: false,
      }),
    ],
  });
  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private alertService: AlertService,
    private dealerSalesCallService: DealerSalesCallService,
    private commonService: CommonService
  ) {
    // this.PAGE_SIZE = 5000;
    // this.ptableSettings.pageSize = 10;
    // this.ptableSettings.enabledServerSitePaggination = false;
    // server side paggination
    this.PAGE_SIZE = commonService.PAGE_SIZE;
    this.ptableSettings.pageSize = this.PAGE_SIZE;
    this.ptableSettings.enabledServerSitePaggination = true;
  }

  ngOnInit() {
    this.searchOptionQuery = new SearchOptionQuery();
    this.searchOptionQuery.clear();

    this.searchConfiguration();
    // of(undefined)
    //   .pipe(take(1), delay(1000))
    //   .subscribe(() => {
    //     this.loadDealerSalesCallsPage();
    //   });
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  loadDealerSalesCallsPage() {
    // this.searchConfiguration();
    this.alertService.fnLoading(true);
    const dealerSalesCallsSubscription = this.dealerSalesCallService
      .getDealerSalesCalls(this.query)
      .pipe(
        finalize(() => {
          this.alertService.fnLoading(false);
        })
        // debounceTime(1000),
        // distinctUntilChanged()
      )
      .subscribe(
        (res) => {
          this.dealerSalesCalls = res.data.items;
          this.totalDataLength = res.data.total;
          this.totalFilterDataLength = res.data.totalFilter;
          this.dealerSalesCalls.forEach((obj) => {
            this.commonService.booleanToText(obj);
          });
          this.dealerSalesCalls.forEach((x) => {
            x.detailsBtnText = 'View Sales Call';
          });
        },
        (error) => {}
      );
    this.subscriptions.push(dealerSalesCallsSubscription);
  }

  searchConfiguration() {
    this.query = new DealerSalesCallQuery({
      page: 1,
      pageSize: this.PAGE_SIZE,
      sortBy: 'userFullName',
      isSortAscending: true,
      globalSearchValue: '',
    });
  }

  // toggleActiveInactive(id) {
  // 	const actInSubscription = this.dealerSalesCallService.activeInactive(id).subscribe(res => {
  // 		this.loadDealerSalesCallsPage();
  // 	});
  // 	this.subscriptions.push(actInSubscription);
  // }

  public ptableSettings: IPTableSetting = {
    tableID: 'dealerSalesCalls-table',
    tableClass: 'table table-border ',
    tableName: 'Dealer Sales Call List',
    tableRowIDInternalName: 'id',
    tableColDef: [
      {
        headerName: 'Employee Name',
        width: '15%',
        internalName: 'userFullName',
        sort: true,
        type: '',
      },
      {
        headerName: 'Dealer Name',
        width: '15%',
        internalName: 'dealerName',
        sort: true,
        type: '',
      },
      {
        headerName: 'Is Target Communicated',
        width: '10%',
        internalName: 'isTargetCommunicatedText',
        sort: false,
        type: '',
      },
      {
        headerName: 'Has OS',
        width: '10%',
        internalName: 'hasOSText',
        sort: false,
        type: '',
      },
      {
        headerName: 'Has Slippage',
        width: '10%',
        internalName: 'hasSlippageText',
        sort: false,
        type: '',
      },
      {
        headerName: 'Is CB Installed',
        width: '10%',
        internalName: 'isCBInstalledText',
        sort: false,
        type: '',
      },
      {
        headerName: 'Has Sub Dealer Influence',
        width: '10%',
        internalName: 'hasSubDealerInfluenceText',
        sort: false,
        type: '',
      },
      {
        headerName: 'Has Painter Influence',
        width: '10%',
        internalName: 'hasPainterInfluenceText',
        sort: false,
        type: '',
      },
      {
        headerName: 'Details',
        width: '10%',
        internalName: 'detailsBtnText',
        sort: false,
        type: 'button',
        onClick: 'true',
        innerBtnIcon: '',
      },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    // pageSize: 10,
    enabledPagination: true,
    // enabledDeleteBtn: true,
    // enabledEditBtn: true,
    enabledCellClick: true,
    enabledColumnFilter: false,
    // enabledRecordCreateBtn: true,
    enabledDataLength: true,
    // newRecordButtonText: 'New ELearning'
  };

  serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
    this.query.page = queryObj.pageNo;
    this.query.pageSize = queryObj.pageSize;
    this.query.sortBy = queryObj.orderBy;
    this.query.isSortAscending = queryObj.isOrderAsc;
    this.query.globalSearchValue = queryObj.searchVal;
    this.loadDealerSalesCallsPage();
  }

  public cellClickCallbackFn(event: any) {
    let id = event.record.id;
    let cellName = event.cellName;

    if (cellName == 'detailsBtnText') {
      this.detailsDealerSalesCall(id);
    }
  }

  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    this.isSearchClick = true;
    this.query.depoId = queryObj.depot;
    this.query.territories = queryObj.territories;
    this.query.custZones = queryObj.zones;
    this.query.salesGroup = queryObj.salesGroups;
    this.query.dealerId = queryObj.dealerId;

    this.loadDealerSalesCallsPage();
  }

  public detailsDealerSalesCall(id) {
    this.router.navigate([`/dealer-sales-call/details/${id}`]);
  }
}
