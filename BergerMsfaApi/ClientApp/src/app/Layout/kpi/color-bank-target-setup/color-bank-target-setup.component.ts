import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { Subscription } from 'rxjs/internal/Subscription';
import { finalize } from 'rxjs/operators';
import { ColorBankTargetSetupKpiReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings,
} from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ColorBankInstallationTargetService } from 'src/app/Shared/Services/KPI/color-bank-installation-target.service';
import { AuthService } from 'src/app/Shared/Services/Users';

@Component({
  selector: 'app-color-bank-target-setup',
  templateUrl: './color-bank-target-setup.component.html',
  styleUrls: ['./color-bank-target-setup.component.css'],
})
export class ColorBankTargetSetupComponent implements OnInit {
  // data list
  query: ColorBankTargetSetupKpiReportQuery;
  searchOptionQuery: SearchOptionQuery;
  PAGE_SIZE: number;
  data: any[];
  totalDataLength: number = 0; // for server side paggination
  totalFilterDataLength: number = 0; // for server side paggination

  depotList = [];
  territoryList = [];

  // ptable settings
  enabledTotal: boolean = false;
  tableName: string = 'Strike rate on business call Report';
  // renameKeys: any = {'userId':'User Id'};
  renameKeys: any = {};
  allTotalKeysOfNumberType: boolean = true;
  // totalKeys: any[] = ['totalCall'];
  totalKeys: any[] = [];
  searchForm: FormGroup;
  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private colorBankInstallationTargetService: ColorBankInstallationTargetService,
    private alertService: AlertService,
    private commonService: CommonService,
    private formBuilder: FormBuilder,
    public authService: AuthService
  ) {}

  ngOnInit() {
    this.populateDropdown();
    this.initCollectionForm();
  }

  initCollectionForm() {
    this.createForm();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  prepareNewDealerEntry(): ColorBankTargetSetupKpiReportQuery {
    const controls = this.searchForm.controls;
    const _query = new ColorBankTargetSetupKpiReportQuery();
    _query.depot = controls['depot'].value;
    _query.territory = controls['territory'].value;
    _query.year = this.commonService.getFiscalYear();
    return _query;
  }

  createForm() {
    this.searchForm = this.formBuilder.group({
      depot: [''],
      territory: [''],
    });
  }

  get formControls() {
    return this.searchForm.controls;
  }
  populateDropdown(): void {
    // this.loadDynamicDropdown();

    const forkJoinSubscription1 = forkJoin([
      this.commonService.getDepotList(),
      this.commonService.getTerritoryList(),
    ]).subscribe(
      ([depot, territory]) => {
        this.depotList = depot.data;
        this.territoryList = territory.data;
      },
      (err) => {},
      () => {}
    );

    this.subscriptions.push(forkJoinSubscription1);
  }

  searchConfiguration() {
    this.query = new ColorBankTargetSetupKpiReportQuery({
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
      new SearchOptionDef({
        searchOption: EnumSearchOption.FiscalYear,
        isRequired: true,
      }),
    ],
  });

  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    console.log('Search option query callback: ', queryObj);
    this.query.depot = queryObj.depot;
    this.query.territories = queryObj.territories;
    this.query.year = queryObj.fiscalYear;
    this.loadData();
  }

  getData = (query) =>
    this.colorBankInstallationTargetService.getCollectionConfigs(query);

  loadData() {
    const query = this.prepareNewDealerEntry();
    this.alertService.fnLoading(true);
    const reportsSubscription = this.getData(query)
      .pipe(
        finalize(() => {
          this.alertService.fnLoading(false);
        })
      )
      .subscribe(
        (res) => {
          this.data = res.data;
          // this.totalDataLength = res.data.length;
          //this.totalFilterDataLength = res.data.length;
          //this.ptableColDefGenerate();
        },
        (error) => {
          console.log(error);
        }
      );
    this.subscriptions.push(reportsSubscription);
  }

  SaveOrUpdateData() {
    this.colorBankInstallationTargetService
      .saveOrUpdateInstallTarget(this.data)
      .subscribe(
        (res) => {
          this.alertService.tosterSuccess(
            'Color Bank Installation Targer Set Successfully'
          );
          this.loadData();
        },
        (error) => console.log(error),
        () => console.log('done')
      );
  }
}
