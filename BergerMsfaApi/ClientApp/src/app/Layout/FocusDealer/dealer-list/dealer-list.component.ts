import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUploader } from 'ng2-file-upload';
import { Paginator } from 'primeng/paginator';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings,
} from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { FocusdealerService } from 'src/app/Shared/Services/FocusDealer/focusdealer.service';
import { AuthService } from 'src/app/Shared/Services/Users';
import { APIModel } from '../../../Shared/Entity';
import { FileUploaderComponent } from '../../file-upload/file-uploader/file-uploader.component';

@Component({
  selector: 'app-dealer-list',
  templateUrl: './dealer-list.component.html',
  styleUrls: ['./dealer-list.component.css'],
})
export class DealerListComponent implements OnInit {
  searchOptionQuery: SearchOptionQuery;
  tableName: string = 'Dealer List';
  data: any[];
  filterObj: any;
  clubSupreme: any[];
  public uploader: FileUploader;
  searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
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
    ],
  });
  first = 1;
  rows = 10;
  pagingConfig: APIModel;
  pageSize: number;
  search: string = '';
  dealerList: any[] = [];
  baseUrl: string;
  @ViewChild('paging', { static: false }) paging: Paginator;

  constructor(
    private dealerSvc: FocusdealerService,
    private alertSvc: AlertService,
    private router: Router,
    private modalService: NgbModal,
    @Inject('BASE_URL') baseUrl: string,
    private authService: AuthService,
    private commonService: CommonService
  ) {
    console.log('baseUrl: ', baseUrl);
    this.baseUrl = baseUrl + 'api/';
    this.pagingConfig = new APIModel(1, 10);
  }

  ngOnInit() {
    this.uploader = new FileUploader({
      authTokenHeader: 'Authorization',
      authToken: 'Bearer ' + this.authService.currentUserToken,
    });

    this.searchOptionQuery = new SearchOptionQuery();
    this.searchOptionQuery.clear();

    this.filterObj = {
      index: this.pagingConfig.pageNumber,
      pageSize: this.pagingConfig.pageSize,
      search: this.search,
      depoId: '',
      customerNo: 0,
      territories: [],
      custZones: [],
      salesGroup: [],
    };

    this.commonService.getEnumClubSupreme().subscribe((res) => {
      this.clubSupreme = res.data.map((x: any) => ({
        label: x.value,
        value: x.id,
      }));
      console.log(this.clubSupreme);
    });
  }
  next() {
    this.pagingConfig.pageNumber =
      this.pagingConfig.pageNumber + this.pagingConfig.pageSize;
    this.OnLoadDealer(this.getFilterObject());
  }
  prev() {
    this.pagingConfig.pageNumber =
      this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
    this.OnLoadDealer(this.getFilterObject());
  }
  onSearch() {
    this.reset();
    this.OnLoadDealer(this.getFilterObject());
  }
  reset() {
    this.paging.first = 1;
    this.pagingConfig = new APIModel(1, 10);
    this.OnLoadDealer(this.getFilterObject());
  }

  isLastPage(): boolean {
    return this.dealerList
      ? this.first === this.dealerList.length - this.rows
      : true;
  }

  isFirstPage(): boolean {
    return this.dealerList ? this.pagingConfig.pageNumber === 1 : true;
  }
  public paginate(event) {
    //let first = Number(event.page) + 1;
    //this.OnLoadDealer(first, event.rows, this.search);
    this.pagingConfig.pageNumber = Number(event.page) + 1;
    this.pagingConfig.pageSize = Number(event.rows);
    this.OnLoadDealer(this.getFilterObject());

    // event.first == 0 ?  1 : event.first;
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
  }

  OnLoadDealer(filterObj: any) {
    this.dealerSvc
      .getDealerList(filterObj)
      .subscribe(
        (res: any) => {
          this.pagingConfig = res.data;
          this.dealerList = this.pagingConfig.model;
        },
        (error) => {
          this.displayError(error);
        }
      )
      .add(() => this.alertSvc.fnLoading(false));
  }

  onChange(value, dealer, property) {
    // if (property == 'isCBInstalled') dealer[property] = value;
    // if (property == 'isExclusive') dealer[property] = value;
    // debugger;

    dealer[property] = value;
    this.alertSvc.fnLoading(true);

    this.dealerSvc
      .updateDealerStatus(dealer)
      .subscribe(
        (res) => {
          this.OnLoadDealer(this.getFilterObject());
        },
        () => {}
      )
      .add(() => this.alertSvc.fnLoading(false));
  }
  private displayError(errorDetails: any) {
    console.log('error', errorDetails);
    let errList = errorDetails.error.errors;
    if (errList.length) {
      console.log('error', errList, errList[0].errorList[0]);
      this.alertSvc.tosterDanger(errList[0].errorList[0]);
    } else {
      this.alertSvc.tosterDanger(errorDetails.error.msg);
    }
  }

  detail(id) {
    this.router.navigate(['/dealer/dealerList/' + id]);
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
    enabledTotal: false,
    enabledExcelDownload: true,
  };

  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    this.filterObj = {
      index: this.pagingConfig.pageNumber,
      pageSize: this.pagingConfig.pageSize,
      search: this.search,
      depoId: queryObj.depot,
      territories: queryObj.territories,
      custZones: queryObj.zones,
      salesGroup: queryObj.salesGroups,
    };

    this.OnLoadDealer(this.getFilterObject());
  }

  getFilterObject() {
    this.filterObj['index'] = this.pagingConfig.pageNumber;
    this.filterObj['pageSize'] = this.pagingConfig.pageSize;
    return this.filterObj;
  }

  uploadExcel(files: File[]) {
    this.uploader.setOptions({
      url: this.baseUrl + 'v1/excel/SubmitExcel',
    });

    const config = {
      backdrop: false,
      ignoreBackdropClick: false,
    };

    const modalRef = this.modalService.open(FileUploaderComponent, config);
    modalRef.componentInstance.uploader = this.uploader;
    modalRef.result.then((result) => {
      if (result === 'close') {
        this.uploader.clearQueue(); // Refresh Data in table grid
      }
    });
  }
}
