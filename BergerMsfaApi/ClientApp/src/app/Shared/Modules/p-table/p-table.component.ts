import { HttpClient } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  DoCheck,
  EventEmitter,
  Input,
  KeyValueDiffers,
  OnInit,
  Output,
  Renderer2,
} from '@angular/core';
import { colDef } from 'src/app/Shared/Modules/p-table';
import { IPTableServerQueryObj, IPTableSetting, ptableStyle } from './p-table';
//import { PTableFilterComponent } from './p-table-pipe';
import { PagerService } from './p-table-pagger';
import { ExcelService } from './service/excel.service';
import { PDFService } from './service/pdf.service';
import { PrintService } from './service/print.service';
declare var jQuery: any;
import { downloadFile } from 'file-saver';
@Component({
  selector: 'app-p-table',
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './p-table.component.html',
  styleUrls: ['./p-table.component.css'],
  providers: [PagerService],
})
export class PTableComponent implements OnInit, DoCheck {
  @Input() pTableSetting: IPTableSetting;
  @Input() pTableMasterData: any[];
  @Input() pTableTotalDataLength: number = 0; // for server side paggination
  @Input() pTableTotalFilterDataLength: number = 0; // for server side paggination
  @Output() checkboxCallbackFn: EventEmitter<any> =
    new EventEmitter<any>() || null;
  @Output() customActivityOnRecord: EventEmitter<any> =
    new EventEmitter<any>() || null;
  @Output() callbackFnOnPageChange: EventEmitter<any> =
    new EventEmitter<any>() || null;
  @Output() radioButtonCallbackFn: EventEmitter<any> =
    new EventEmitter<any>() || null;
  @Output() cellClickCallbackFn: EventEmitter<any> =
    new EventEmitter<any>() || null;
  @Output() customReflowFn: EventEmitter<any> = new EventEmitter<any>() || null;
  @Output() serverSiteCallbackFn: EventEmitter<IPTableServerQueryObj> =
    new EventEmitter<IPTableServerQueryObj>() || null;
  Math: any;
  public editUpdateColumn: boolean = true;
  public noRecord = true;
  public pageSize: number;
  public showingPageDetails: string;
  public pTableData: any[] = [{}];
  public pTableDatalength: number = 0;
  public startPageNo: number = 0;
  public totalColspan = 0;
  public maximumPaggingDisplay: number;
  public pageNo: number = 0;
  public differ: any;
  public rowLimitArray: any[] = [10, 20, 50, 100, 200, 500, 1000];
  public enabledPagination: boolean = true;
  public globalSearchValue: string = '';
  public orderBy: string = '';
  public isOrderAsc: boolean | true;

  //for table smart filter
  public filterCustomColumnName: string;
  public filterColumnTitle: string;
  public customFilterUniqueArray: any[] = [];
  public columnWiseMasterData: any[] = [];
  public filterItemsCheckedAll: boolean = false;
  public popupFilterColor: string = 'black';
  public storedFilteredInfo: any[] = [];
  public columnSearchValue: string = '';
  public activeReflow: boolean = false;
  public customReflowActive: boolean = false;
  public pTableColumnSearch: string = '';
  public pTableColumnCustomizationList: any[] = [];
  public pTableColumnReorder: any[] = [];
  public settingsTabs: any[] = [
    { tab: 'columnShowHide', tabName: 'Show/Hide', active: true },
    { tab: 'columnOrder', tabName: 'Order Change', active: true },
  ];

  public pModalSetting: any = {
    modalTitle: '',
    modalSaveBtnCaption: 'Save',
  };
  pager: any = {};
  pagedItems: any[];
  constructor(
    private pagerService: PagerService,
    private differs: KeyValueDiffers,
    public renderer: Renderer2,
    private pdfService: PDFService,
    private excelService: ExcelService,
    private printService: PrintService,
    private http: HttpClient
  ) {
    this.Math = Math;
    this.differ = differs.find({}).create();
  }

  ngOnInit() {
    if (this.pTableSetting == null) {
      //to return without values
      return false;
    }
    this.pTableSetting.tableID = Math.floor(
      100000 + Math.random() * 900000
    ).toString();
    console.log('this.pTableSetting.tableID:', this.pTableSetting.tableID);
    if (this.pTableSetting['enabledSerialNo']) {
      this.totalColspan = this.totalColspan + 1;
    }
    if (this.pTableSetting['enabledCheckbox']) {
      this.totalColspan = this.totalColspan + 1;
    }
    if (this.pTableSetting['enabledEditDeleteBtn'] || this.pTableSetting['enabledEditBtn'] || this.pTableSetting['enabledDeleteBtn'] || 
        this.pTableSetting['enabledDetailsEditDeleteBtn'] || this.pTableSetting['enabledDetailsBtn']) {
      this.totalColspan = this.totalColspan + 1;
    }

    if (this.pTableSetting['enabledRadioBtn']) {
      this.totalColspan = this.totalColspan + 1;
    }
    if (this.pTableSetting['enabledReordering']) {
      this.settingsTabs.push({
        tab: 'columnOrder',
        tabName: 'Reorder',
        active: false,
      });
    }
    this.pTableSetting['radioBtnColumnHeader'] =
      this.pTableSetting['radioBtnColumnHeader'] || ' ';
    this.pTableSetting['checkboxColumnHeader'] =
      this.pTableSetting['checkboxColumnHeader'] || ' ';

    this.totalColspan =
      this.totalColspan + this.pTableSetting['tableColDef'].length;
    this.maximumPaggingDisplay = this.pTableSetting['displayPaggingSize'] || 10;
    if (this.pTableSetting['enabledAutoScrolled']) {
      this.enabledPagination = false;
      this.pageSize = this.pTableSetting['pageSize'] || 30;
    } else if (
      !this.pTableSetting['enabledPagination'] &&
      this.pTableSetting['enabledPagination'] != undefined
    ) {
      this.enabledPagination = false;
      this.pageSize = 30000;
    } else {
      this.pageSize = this.pTableSetting['pageSize'] || 10;
    }

    //for advanced column filter
    this.storedFilteredInfo = [];
    this.columnSearchValue = '';
    this.globalSearchValue = '';
    jQuery('#' + this.pTableSetting['tableID'] + ' .column-filter-active').css(
      'color',
      'white'
    );

    this.pTableColumnCustomizationList =
      JSON.parse(JSON.stringify(this.pTableSetting.tableColDef)) || [];
    this.pTableColumnCustomizationList.forEach((res) => {
      res.visible = res.visible || true;
    });
    this.pTableColumnReorder =
      JSON.parse(JSON.stringify(this.pTableSetting.tableColDef)) || [];
  }

  ngDoCheck() {
    if (this.pTableSetting == null) {
      //to return without values
      return false;
    }
    var changes = this.differ.diff(this.pTableMasterData);
    if (changes) {
      this.pTableData = this.pTableMasterData || [];
      if (this.pTableSetting.enabledServerSitePaggination) {
        this.pTableDatalength = this.pTableTotalFilterDataLength;
      } else {
        this.pTableDatalength = this.pTableData.length || 0;
      }
      this.selectedCheckedItems = [];

      if (this.pTableSetting.disabledTableReset) {
        this.fnShowPreviousFilteredState();
      } else {
        this.storedFilteredInfo = [];
        this.columnSearchValue = '';
        if (!this.pTableSetting.enabledServerSitePaggination) {
          this.globalSearchValue = '';
        }
        jQuery(
          '#' + this.pTableSetting['tableID'] + ' .column-filter-active'
        ).css('color', 'white');
      }

      if (this.pTableSetting.enabledServerSitePaggination) {
        this.setPage(this.pageNo);
      } else {
        //set page state
        if (this.pTableSetting.enabledStaySelectedPage && this.pageNo > 0) {
          this.setPage(this.pageNo);
        } else {
          this.setPage(1);
        }
      }
    }
  }

  fnClickPTableCell(
    event: any,
    isCellClick: boolean = false,
    currentCellName: string,
    activeClickForThisCell: string,
    data: any
  ) {
    if (
      isCellClick &&
      (activeClickForThisCell == 'Yes' || activeClickForThisCell == 'true')
    ) {
      this.cellClickCallbackFn.emit({
        cellName: currentCellName,
        record: data,
        event: event,
      });
    } else {
      return;
    }
  }

  fnSaveModalInfo() {
    // this.fnActionOnSaveBtn.emit(this.modalSaveFnParam);
  }

  fnEditRecord(record: any) {
    //jQuery("#customModal").modal("show");
  }

  fnDeleteRecord(record: any) {}
  async fnFilterPTable(args: any, executionType: boolean = false) {
    let execution = false;
    args = args.trim();

    if (this.pTableSetting.enabledServerSitePaggination) {
      this.pageNo = 1;
      const queryObj: IPTableServerQueryObj = {
        searchVal: args,
        pageNo: this.pageNo,
        pageSize: this.pageSize,
        orderBy: this.orderBy,
        isOrderAsc: this.isOrderAsc,
      };
      this.serverSiteCallbackFn.emit(queryObj);
    } else {
      //this.pTableData=JSON.parse( JSON.stringify( this.pTableMasterData))||[];
      if (args && this.pTableMasterData.length > 0) {
        let filterKeys = Object.keys(this.pTableMasterData[0]);
        this.pTableData = await this.pTableMasterData.filter(
          (item: any, index: any, array: any) => {
            let returnVal = false;
            for (let i = 0; i < this.pTableSetting['tableColDef'].length; i++) {
              if (
                typeof item[
                  this.pTableSetting['tableColDef'][i]['internalName']
                ] == 'string'
              ) {
                if (
                  item[this.pTableSetting['tableColDef'][i]['internalName']]
                    .toLowerCase()
                    .includes(args.toLowerCase())
                ) {
                  returnVal = true;
                }
              } else if (
                typeof item[
                  this.pTableSetting['tableColDef'][i]['internalName']
                ] == 'number'
              ) {
                if (
                  item[this.pTableSetting['tableColDef'][i]['internalName']]
                    .toString()
                    .indexOf(args.toString()) > -1
                ) {
                  returnVal = true;
                }
              } else {
                //returnVal = false;
              }
            }

            return returnVal;
          }
        );
      } else {
        this.pTableData = this.pTableMasterData;
      }

      if (executionType) {
      } else {
        this.storedFilteredInfo = [];
        jQuery(
          '#' + this.pTableSetting['tableID'] + ' .column-filter-active'
        ).css('color', 'white');
        this.setPage(1);
      }
    }
  }

  setPage(page: number, isPageChanged: boolean = false) {
    this.pageNo = page;
    if (this.pTableSetting.enabledServerSitePaggination) {
      this.pager = this.pagerService.getPager(
        this.pTableTotalFilterDataLength,
        page,
        this.pageSize,
        this.maximumPaggingDisplay
      );
    } else {
      this.pager = this.pagerService.getPager(
        this.pTableData.length,
        page,
        this.pageSize,
        this.maximumPaggingDisplay
      );
    }
    if (page < 1 || page > this.pager.totalPages) {
      if (page - 1 <= this.pager.totalPages && this.pager.totalPages != 0) {
        if (page <= 0) {
          this.setPage(1);
        } else {
          this.setPage(page - 1);
        }
        return;
      }
    }
    //this.pager = this.pagerService.getPager(this.pTableData.length, page, this.pageSize, this.maximumPaggingDisplay);
    if (this.pTableSetting.enabledServerSitePaggination) {
      if (this.pTableData.length == 0) {
        this.pagedItems = [];
      } else {
        this.pagedItems = this.pTableData;
      }

      this.pTableDatalength = this.pTableTotalFilterDataLength;
      //showing page number
      this.startPageNo = (this.pager.currentPage - 1) * this.pager.pageSize + 1;
      let endPageNo = 0;
      if (this.pTableTotalFilterDataLength == 0) {
        this.startPageNo = 0;
      }

      if (
        this.pager.currentPage * this.pager.pageSize <
        this.pTableTotalFilterDataLength
      ) {
        endPageNo = this.pager.currentPage * this.pager.pageSize;
      } else {
        endPageNo = this.pTableTotalFilterDataLength;
      }

      if (this.pTableTotalFilterDataLength == this.pTableTotalDataLength) {
        this.showingPageDetails =
          'Showing ' +
          this.startPageNo +
          ' to ' +
          endPageNo +
          ' of ' +
          this.pTableTotalFilterDataLength +
          ' records';
      } else {
        this.showingPageDetails =
          'Showing ' +
          this.startPageNo +
          ' to ' +
          endPageNo +
          ' of ' +
          this.pTableTotalFilterDataLength +
          ' records (filtered from ' +
          this.pTableTotalDataLength +
          ' total records)';
      }
    } else {
      if (this.pTableData.length == 0) {
        this.pagedItems = [];
      } else {
        this.pagedItems = this.pTableData.slice(
          this.pager.startIndex,
          this.pager.endIndex + 1
        );
      }

      this.pTableDatalength = this.pTableData.length;
      //showing page number
      this.startPageNo = (this.pager.currentPage - 1) * this.pager.pageSize + 1;
      let endPageNo = 0;
      if (this.pTableData.length == 0) {
        this.startPageNo = 0;
      }

      if (
        this.pager.currentPage * this.pager.pageSize <
        this.pTableData.length
      ) {
        endPageNo = this.pager.currentPage * this.pager.pageSize;
      } else {
        endPageNo = this.pTableData.length;
      }

      if (this.pTableData.length == this.pTableMasterData.length) {
        this.showingPageDetails =
          'Showing ' +
          this.startPageNo +
          ' to ' +
          endPageNo +
          ' of ' +
          this.pTableData.length +
          ' records';
      } else {
        this.showingPageDetails =
          'Showing ' +
          this.startPageNo +
          ' to ' +
          endPageNo +
          ' of ' +
          this.pTableData.length +
          ' records (filtered from ' +
          this.pTableMasterData.length +
          ' total records)';
      }
    }
    //call the function after the page changes
    this.callbackFnOnPageChange.emit({ pageNo: page });

    if (this.pTableSetting.enabledServerSitePaggination && isPageChanged) {
      const queryObj: IPTableServerQueryObj = {
        searchVal: this.globalSearchValue,
        pageNo: page,
        pageSize: this.pageSize,
        orderBy: this.orderBy,
        isOrderAsc: this.isOrderAsc,
      };
      this.serverSiteCallbackFn.emit(queryObj);
    }
  }

  //#region checkbox & radio btn
  public selectedCheckedItems: any[] = [];
  public fnOperationOnCheckBox(event: any, args: string) {
    if (event.target.checked) {
      this.selectedCheckedItems = this.pTableData || [];
    } else {
      this.selectedCheckedItems = [];
    }

    this.cellClickCallbackFn.emit({
      cellName: 'check-box',
      record: this.selectedCheckedItems,
      event: event,
    });
  }

  public fnGetCheckedValue(row: any): boolean {
    return (
      this.selectedCheckedItems.filter(
        (r) =>
          r[this.pTableSetting.tableRowIDInternalName] ==
          row[this.pTableSetting.tableRowIDInternalName]
      ).length > 0
    );
  }

  fnIndividualCheckboxAction(e: any, recordInfo: any) {
    if (e.target.checked) {
      let row: any[] = [recordInfo];
      this.selectedCheckedItems = this.selectedCheckedItems.concat(row);
    } else {
      this.selectedCheckedItems = this.selectedCheckedItems.filter(
        (r) =>
          r[this.pTableSetting.tableRowIDInternalName] !=
          recordInfo[this.pTableSetting.tableRowIDInternalName]
      );
    }

    this.cellClickCallbackFn.emit({
      cellName: 'check-box',
      record: this.selectedCheckedItems,
      event: e,
    });
  }

  public fnIndividualRadioAction(e: any, recordInfo: any) {
    e.target.checked
      ? (this.selectedCheckedItems = [recordInfo])
      : (this.selectedCheckedItems = []);
    this.cellClickCallbackFn.emit({
      cellName: 'radio-btn',
      record: this.selectedCheckedItems,
      event: e,
    });
  }

  //#endregion checkbox & radio btn

  public fnActivityOnRecord(action: any, recordInfo: any) {
    this.customActivityOnRecord.emit({ action: action, record: recordInfo });
  }

  public fnChangePTableRowLength(records: number) {
    this.pageSize = +records;
    if (this.pTableSetting.enabledServerSitePaggination) {
      this.pageNo = 1;
      const queryObj: IPTableServerQueryObj = {
        searchVal: this.globalSearchValue,
        pageNo: this.pageNo,
        pageSize: this.pageSize,
        orderBy: this.orderBy,
        isOrderAsc: this.isOrderAsc,
      };
      this.serverSiteCallbackFn.emit(queryObj);
    } else {
      this.setPage(1);
    }
  }

  public fnChangePTableDataLength(event: any) {
    let records = event.target.value;
    this.pageSize = records;
    this.setPage(1);
  }

  public start: any;
  public pressed: any;
  public startX: any;
  public startWidth: any;

  public fnResizeColumn(event: any) {
    this.start = event.target;
    this.pressed = true;
    this.startX = event.x;
    this.startWidth = jQuery(this.start).parent().width();
    this.initResizableColumns();
  }

  public initResizableColumns() {
    this.renderer.listen('body', 'mousemove', (event: any) => {
      if (this.pressed) {
        let width = this.startWidth + (event.x - this.startX);
        jQuery(this.start)
          .parent()
          .css({ 'min-width': width, 'max-   width': width });
        let index = jQuery(this.start).parent().index() + 1;
        jQuery(
          '#' + this.pTableSetting.tableID + ' tr td:nth-child(' + index + ')'
        ).css({ 'min-width': width, 'max-width': width });
      }
    });
    this.renderer.listen('body', 'mouseup', (event: any) => {
      if (this.pressed) {
        this.pressed = false;
      }
    });
  }

  //#region  sorting & searching
  public fnColumnSorting(
    colName: any,
    pTableID: any,
    isSorting: boolean = true
  ) {
    if (this.pTableSetting.enabledServerSitePaggination) {
      if (!isSorting || this.pTableTotalFilterDataLength < 1) {
        return;
      }

      let isOrderAsc = true;

      if (jQuery('#' + pTableID + ' thead th.' + colName).hasClass('sorting')) {
        jQuery('#' + pTableID + ' thead th.sorting-active')
          .addClass('sorting')
          .removeClass('sorting-down')
          .removeClass('sorting-up');
        jQuery('#' + pTableID + ' thead th.' + colName)
          .addClass('sorting-up')
          .removeClass('sorting');
        isOrderAsc = true;
      } else if (
        jQuery('#' + pTableID + ' thead th.' + colName).hasClass('sorting-up')
      ) {
        jQuery('#' + pTableID + ' thead th.' + colName)
          .addClass('sorting-down')
          .removeClass('sorting-up');
        isOrderAsc = false;
      } else if (
        jQuery('#' + pTableID + ' thead th.' + colName).hasClass('sorting-down')
      ) {
        jQuery('#' + pTableID + ' thead th.' + colName)
          .addClass('sorting-up')
          .removeClass('sorting-down');
        isOrderAsc = true;
      }

      this.orderBy = colName;
      this.isOrderAsc = isOrderAsc;
      this.pageNo = 1;
      const queryObj: IPTableServerQueryObj = {
        searchVal: this.globalSearchValue,
        pageNo: this.pageNo,
        pageSize: this.pageSize,
        orderBy: this.orderBy,
        isOrderAsc: this.isOrderAsc,
      };
      this.serverSiteCallbackFn.emit(queryObj);
    } else {
      if (!isSorting || this.pTableMasterData.length < 1) {
        return;
      }

      if (jQuery('#' + pTableID + ' thead th.' + colName).hasClass('sorting')) {
        jQuery('#' + pTableID + ' thead th.sorting-active')
          .addClass('sorting')
          .removeClass('sorting-down')
          .removeClass('sorting-up');
        jQuery('#' + pTableID + ' thead th.' + colName)
          .addClass('sorting-up')
          .removeClass('sorting');
        this.pTableData = this.pTableData.sort((n1, n2) => {
          if (n1[colName] > n2[colName]) {
            return 1;
          }
          if (n1[colName] < n2[colName]) {
            return -1;
          }
          return 0;
        });
      } else if (
        jQuery('#' + pTableID + ' thead th.' + colName).hasClass('sorting-up')
      ) {
        jQuery('#' + pTableID + ' thead th.' + colName)
          .addClass('sorting-down')
          .removeClass('sorting-up');
        this.pTableData = this.pTableData.sort((n1, n2) => {
          if (n1[colName] < n2[colName]) {
            return 1;
          }
          if (n1[colName] > n2[colName]) {
            return -1;
          }
          return 0;
        });
      } else if (
        jQuery('#' + pTableID + ' thead th.' + colName).hasClass('sorting-down')
      ) {
        jQuery('#' + pTableID + ' thead th.' + colName)
          .addClass('sorting-up')
          .removeClass('sorting-down');
        this.pTableData = this.pTableData.sort((n1, n2) => {
          if (n1[colName] > n2[colName]) {
            return 1;
          }
          if (n1[colName] < n2[colName]) {
            return -1;
          }
          return 0;
        });
      }
      this.setPage(1);
    }
  }

  public fnIndividualColumnFilterContext(columnDef: any, event: any) {
    this.filterCustomColumnName = columnDef.internalName;
    this.filterColumnTitle = columnDef.headerName;
    this.columnSearchValue = '';
    this.columnWiseMasterData =
      this.fnFindUniqueColumnWithCheckedFlag(
        this.pTableData,
        this.filterCustomColumnName
      ) || [];
    this.customFilterUniqueArray = JSON.parse(
      JSON.stringify(this.columnWiseMasterData)
    );
    //to checked all
    this.filterItemsCheckedAll = true;

    //to set color of filter popup icon
    let checkFilterApplied =
      this.storedFilteredInfo.filter((rec: any) => {
        if (rec.columnName == this.filterCustomColumnName) {
          return true;
        } else {
          return false;
        }
      }) || [];
    this.popupFilterColor = 'black';
    if (checkFilterApplied.length > 0) {
      this.popupFilterColor = 'red';
    }
  }

  public fnCustomFilterSelectAll(event: any) {
    if (event.target.checked) {
      this.customFilterUniqueArray.forEach((rec: any) => {
        rec.checked = true;
      });
    } else {
      this.customFilterUniqueArray.forEach((rec: any) => {
        rec.checked = false;
      });
    }
  }

  public fnApplyCustomFilter() {
    this.pTableData =
      this.fnCustomFilterFromMasterArray(
        this.pTableData,
        this.filterCustomColumnName,
        this.customFilterUniqueArray.filter((rec: any) => rec.checked == true)
      ) || [];
    jQuery(
      '#' +
        this.pTableSetting['tableID'] +
        ' #filter-icon-' +
        this.filterCustomColumnName
    ).css('color', 'red');
    jQuery('#' + this.pTableSetting.tableID + '-fitlerInfo').hide();
    if (this.storedFilteredInfo.length > 0) {
      this.storedFilteredInfo =
        this.storedFilteredInfo.filter((rec: any) => {
          if (rec.columnName == this.filterCustomColumnName) {
            return false;
          } else {
            return true;
          }
        }) || [];
      this.storedFilteredInfo.push({
        columnName: this.filterCustomColumnName,
        checkedItem: this.customFilterUniqueArray.filter((rec: any) => {
          if (rec.checked) {
            return true;
          } else {
            return false;
          }
        }),
      });
    } else {
      this.storedFilteredInfo.push({
        columnName: this.filterCustomColumnName,
        checkedItem: this.customFilterUniqueArray.filter((rec: any) => {
          if (rec.checked) {
            return true;
          } else {
            return false;
          }
        }),
      });
    }

    this.setPage(1);
  }

  fnFilterPTableColumn(arg: string) {
    if (this.columnSearchValue.trim() != '') {
      this.customFilterUniqueArray =
        this.columnWiseMasterData.filter((rec: any) => {
          if (
            (rec.data || '')
              .toString()
              .toLowerCase()
              .includes((this.columnSearchValue || '').toLowerCase())
          ) {
            return true;
          } else {
            return false;
          }
        }) || [];
    } else {
      this.customFilterUniqueArray = JSON.parse(
        JSON.stringify(this.columnWiseMasterData)
      );
    }
  }

  fnCustomFilterFromMasterArray(
    masterObject: any[],
    findKey: any,
    data: any[]
  ): any[] {
    var o = {},
      i,
      outer: any,
      l = masterObject.length,
      filteredData: any = [];
    for (outer = 0; outer < data.length; outer++) {
      let filterMasterData =
        masterObject.filter(
          (record: any) => record['' + findKey + ''] == data[outer]['data']
        ) || [];
      filteredData = filteredData.concat(filterMasterData);
    }
    //console.log(filteredData)
    this.filterItemsCheckedAll = true;
    return filteredData;
  }

  // global search
  public fnPTableColumnCustomizationSearch(searchVal: string) {
    this.pTableColumnCustomizationList =
      this.pTableSetting.tableColDef.filter((record: any) => {
        if (record.headerName.toLowerCase().includes(searchVal.toLowerCase())) {
          return true;
        } else {
          return false;
        }
      }) || [];
  }

  public fnCloseCustomFilter() {
    //jQuery("#fitlerInfo").hide();
    jQuery('#' + this.pTableSetting.tableID).click();
  }

  //#endregion  sorting & searching

  //#region  custom settings

  async fnApplyCustomCustomization() {
    this.pTableSetting.tableColDef.forEach((rec: any) => {
      let columnLooping =
        this.pTableColumnCustomizationList.filter((record: any) => {
          if (record.internalName == rec.internalName) {
            return true;
          } else {
            return false;
          }
        }) || [];
      if (columnLooping.length > 0) {
        rec.visible = columnLooping[0].visible;
      } else {
        rec.visible = false;
      }
    });

    //assign again
    if (this.storedFilteredInfo.length > 0) {
      this.pTableData = JSON.parse(JSON.stringify(this.pTableMasterData)) || [];
      this.storedFilteredInfo.forEach((rec: any) => {
        jQuery(
          '#' +
            this.pTableSetting['tableID'] +
            ' #filter-icon-' +
            rec.columnName
        ).css('color', 'white');
      });
      this.storedFilteredInfo = [];
      this.setPage(1);
    }

    //await this.fnShowPreviousFilteredState();
    this.pTableColumnCustomizationList = JSON.parse(
      JSON.stringify(this.pTableSetting.tableColDef)
    );
    this.pTableColumnReorder =
      JSON.parse(JSON.stringify(this.pTableSetting.tableColDef)) || [];
  }

  public activeTabName: string = 'columnShowHide';
  selectTab(tab: any) {
    this.settingsTabs.forEach((rec: any) => {
      if (rec.tab == tab.tab) {
        rec.active = true;
      } else {
        rec.active = false;
      }
    });
    this.activeTabName = tab.tab;
  }

  public fnChangeColumnOrder(colDef: any, index: any, status: string) {
    let old_index = index;
    let new_index: number = 0;
    if (index <= 0 && status == 'up') {
      return false;
    } else if (
      index >= this.pTableColumnReorder.length - 1 &&
      status == 'down'
    ) {
      return false;
    }

    if (status == 'up') {
      new_index = index - 1;
    } else {
      new_index = index + 1;
    }

    if (new_index >= this.pTableColumnReorder.length) {
      var k = new_index - this.pTableColumnReorder.length;
      while (k-- + 1) {
        this.pTableColumnReorder.push(undefined);
      }
    }
    this.pTableColumnReorder.splice(
      new_index,
      0,
      this.pTableColumnReorder.splice(old_index, 1)[0]
    );
  }

  public fnApplyReorderColumn() {
    this.pTableSetting.tableColDef = JSON.parse(
      JSON.stringify(this.pTableColumnReorder)
    );
    this.pTableColumnCustomizationList =
      JSON.parse(JSON.stringify(this.pTableSetting.tableColDef)) || [];
  }

  public onDrop(src: any, trg: any) {
    this.fnModeDragDropContent(
      this.pTableColumnReorder
        .map((x) => x.internalName)
        .indexOf(src.internalName),
      this.pTableColumnReorder
        .map((x) => x.internalName)
        .indexOf(trg.internalName)
    );
  }

  public fnModeDragDropContent(src: any, trg: any) {
    src = parseInt(src);
    trg = parseInt(trg);

    if (trg >= this.pTableColumnReorder.length) {
      var k = trg - this.pTableColumnReorder.length;
      while (k-- + 1) {
        this.pTableColumnReorder.push(undefined);
      }
    }
    this.pTableColumnReorder.splice(
      trg,
      0,
      this.pTableColumnReorder.splice(src, 1)[0]
    );
    return this; // for testing purposes
  }

  //#endregion custom settings

  public tempStyle: ptableStyle[] = [];

  fnFindUniqueColumnWithCheckedFlag(objectSet: any[], findKey: any): any[] {
    var o = {},
      i,
      l = objectSet.length,
      r = [];
    for (i = 0; i < l; i++) {
      o[objectSet[i][findKey]] = objectSet[i][findKey];
    }
    for (i in o) r.push({ checked: true, data: o[i] });
    return r;
  }

  async clearFilterFromFilterPopup() {
    this.pTableData = JSON.parse(JSON.stringify(this.pTableMasterData));
    if (this.globalSearchValue.trim() != '') {
      await this.fnFilterPTable(this.globalSearchValue, true);
    }
    //to remove filter from storedFilteredInfo variable
    if (this.storedFilteredInfo.length > 0) {
      this.storedFilteredInfo =
        this.storedFilteredInfo.filter((rec: any) => {
          if (rec.columnName == this.filterCustomColumnName) {
            return false;
          } else {
            return true;
          }
        }) || [];
    }

    if (this.storedFilteredInfo.length > 0) {
      this.storedFilteredInfo.forEach((rec: any) => {
        this.pTableData =
          this.fnCustomFilterFromMasterArray(
            this.pTableData,
            rec.columnName,
            rec.checkedItem
          ) || [];
      });
    }

    jQuery(
      '#' +
        this.pTableSetting['tableID'] +
        ' #filter-icon-' +
        this.filterCustomColumnName
    ).css('color', 'white');
    jQuery('#' + this.pTableSetting.tableID + '-fitlerInfo').hide();
    this.setPage(1);
  }

  async fnShowPreviousFilteredState() {
    if (this.storedFilteredInfo.length > 0) {
      this.storedFilteredInfo.forEach((rec: any) => {
        this.pTableData =
          this.fnCustomFilterFromMasterArray(
            this.pTableData,
            rec.columnName,
            rec.checkedItem
          ) || [];
        jQuery(
          '#' +
            this.pTableSetting['tableID'] +
            ' #filter-icon-' +
            rec.columnName
        ).css('color', 'red');
      });
    }
    // this.setPage(1);
  }

  fnReflowTable() {
    if (this.pTableSetting.enabledCustomReflow) {
      if (this.customReflowActive) {
        this.customReflowActive = false;
        this.fnResetStyle('retrive');
      } else {
        this.customReflowActive = true;
        this.fnResetStyle('reset');
      }
      this.customReflowFn.emit(this.pTableSetting.tableID);
    } else {
      if (this.activeReflow) {
        jQuery('#' + this.pTableSetting.tableID + '-fitlerInfo').hide();
        this.activeReflow = false;
        this.fnResetStyle('retrive');
      } else {
        this.fnResetStyle('reset');
        this.activeReflow = true;
      }
    }
  }

  fnResetStyle(action: string) {
    if (action == 'reset') {
      //remove previous style
      //if (this.pTableSetting.pTableStyle.overflowContentWidth != undefined && this.pTableSetting.pTableStyle.overflowContentWidth != null) {
      if (
        this.pTableSetting.pTableStyle != undefined &&
        this.pTableSetting.pTableStyle != null
      ) {
        this.tempStyle = [
          {
            tableOverflow:
              this.pTableSetting.pTableStyle.tableOverflow || false,
            tableOverflowX:
              this.pTableSetting.pTableStyle.tableOverflowX || false,
            tableOverflowY:
              this.pTableSetting.pTableStyle.tableOverflowY || false,
            overflowContentWidth:
              this.pTableSetting.pTableStyle.overflowContentWidth || null,
            overflowContentHeight:
              this.pTableSetting.pTableStyle.overflowContentHeight || null,
          },
        ];
        this.pTableSetting.pTableStyle.overflowContentWidth = null;
        this.pTableSetting.pTableStyle.tableOverflowY = true;
        this.pTableSetting.pTableStyle.tableOverflow = false;
      }
    } else if (action == 'retrive') {
      //to reset previous style
      if (this.tempStyle.length > 0) {
        this.pTableSetting.pTableStyle.overflowContentWidth = this.tempStyle[0].overflowContentWidth;
        this.pTableSetting.pTableStyle.overflowContentHeight = this.tempStyle[0].overflowContentHeight;
        this.pTableSetting.pTableStyle.tableOverflow = this.tempStyle[0].tableOverflow;
        this.pTableSetting.pTableStyle.tableOverflowX = this.tempStyle[0].tableOverflowX;
        this.pTableSetting.pTableStyle.tableOverflowY = this.tempStyle[0].tableOverflowY;
      }
    }
  }

  public fnDownloadPDF() {
    if (
      this.pTableSetting.enabledServerSitePaggination &&
      this.pTableSetting.downloadDataApiUrl
    ) {
      this.http.get<any>(`${this.pTableSetting.downloadDataApiUrl}`).subscribe(
        (res) => {
          let data = res as any[];
          this.printService.printContent(this.pTableSetting, data);
        },
        (err) => {
          console.error(err);
        }
      );
    } else {
      this.printService.printContent(this.pTableSetting, this.pTableData);
    }
  }

  public fnPrintPTable() {
    if (
      this.pTableSetting.enabledServerSitePaggination &&
      this.pTableSetting.downloadDataApiUrl
    ) {
      this.http.get<any>(`${this.pTableSetting.downloadDataApiUrl}`).subscribe(
        (res) => {
          let data = res as any[];
          this.printService.printContent(this.pTableSetting, data);
        },
        (err) => {
          console.error(err);
        }
      );
    } else {
      this.printService.printContent(this.pTableSetting, this.pTableData);
    }
  }

  // public fnExeclDownload() {
  //   if (
  //     this.pTableSetting.enabledServerSitePaggination &&
  //     this.pTableSetting.downloadDataApiUrl
  //   ) {
  //     this.http.get<any>(`${this.pTableSetting.downloadDataApiUrl}`).subscribe(
  //       (res) => {
  //         if(this.pTableSetting.downloadFileFromServer) {
  //           console.log(res);
  //         }
  //         else{
  //           let data = res as any[];

  //         this.excelService.exportAsExcelFile(data, this.pTableSetting);
  //         }
          
  //       },
  //       (err) => {
  //         console.error(err);
  //       }
  //     );
  //   } else {

  //     if(this.pTableSetting.downloadFileFromServer) {
  //       this.http.get<any>(`${this.pTableSetting.downloadDataApiUrl}`,
  //       { responseType: 'blob' as 'json' }
           
  //     )
  //     .subscribe(
  //         (res) => {
  //           // console.log(res);
  //           // downloadFile(res,'SnapShotResult.xlsx')
  //           const blob = new Blob([res], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
  //           const url = window.URL.createObjectURL(blob);
  //           // window.open(url);
  //           var link=document.createElement('a');
  //               link.href=window.URL.createObjectURL(blob);
  //               link.download="SnapShotResult.xlsx";
  //               link.click();
            
  //         },
  //         (err) => {
  //           console.error(err);
  //         }
  //       );
  //     }
      
  //     else{
  //       this.excelService.exportAsExcelFile(this.pTableData, this.pTableSetting);
  //     }
  //   }
  // }

  public fnExeclDownload() {
    if (
      this.pTableSetting.downloadFileFromServer &&
      this.pTableSetting.downloadDataApiUrl
    ) {
      this.http.get<any>(`${this.pTableSetting.downloadDataApiUrl}`, { responseType: 'blob' as 'json' }).subscribe(
        (res) => {
          const blob = new Blob([res], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
          const url = window.URL.createObjectURL(blob);
          var link=document.createElement('a');
          link.href=window.URL.createObjectURL(blob);
          link.download="Report.xlsx";
          link.click();
        },
        (err) => {
          console.error(err);
        }
      );
    } else if (
      this.pTableSetting.enabledServerSitePaggination &&
      this.pTableSetting.downloadDataApiUrl
    ) {
      this.http.get<any>(`${this.pTableSetting.downloadDataApiUrl}`).subscribe(
        (res) => {
          let data = res as any[];
          this.excelService.exportAsExcelFile(data, this.pTableSetting);
        },
        (err) => {
          console.error(err);
        }
      );
    } else {
      this.excelService.exportAsExcelFile(this.pTableData, this.pTableSetting);
    }
  }

  onScroll(event, doc) {
    if (this.pTableSetting.enabledAutoScrolled) {
      const scrollBottom = event.target.scrollHeight;
      const scrollTop = event.target.scrollTop;
      const scrollHeight = event.target.scrollHeight;
      const offsetHeight = event.target.offsetHeight;
      const scrollPosition = scrollTop + offsetHeight;
      const pageHeight = window.screen.height;
      const scrollTreshold = scrollHeight - pageHeight;
      //console.log("scroll bottom "+(scrollBottom - scrollTop)+" offsetHeight"+offsetHeight);
      if (Math.round(scrollBottom - scrollTop) == offsetHeight) {
        this.pageSize = this.pageSize + 20;
        this.setPage(1);
      }
    }
  }

  fnSummationTotal(columnName: string) {
    let sum: number = 0;
    this.pTableData.forEach((rec: any) => {
      sum =
        Number(sum) +
          Number(
            rec[columnName] == null || rec[columnName] == ''
              ? 0
              : isNaN(rec[columnName]) == true
              ? 0
              : rec[columnName] || 0
          ) || 0;
    });
    // return sum.toFixed(2);
    return sum;
  }

  public fnCreateNewRecord() {
    this.customActivityOnRecord.emit({ action: 'new-record', record: null });
  }

  getHeader() {
    const cc: colDef[] = [];
    this.pTableSetting.tableColDef.forEach((element) => {
      if (!element.parentHeaderName) {
        cc.push(element);
      } else {
        const copy = { ...element };
        copy.headerName = copy.parentHeaderName;
        copy.colspan = 1;
        if (
          cc.filter((x) => x.headerName == copy.parentHeaderName).length == 0
        ) {
          cc.push(copy);
        } else {
          const temp = cc.filter(
            (x) => x.headerName == copy.parentHeaderName
          )[0];
          temp.colspan = temp.colspan + 1;
        }
      }
    });
    return cc;
  }

  isParentHeaderExists() {
    return (
      this.pTableSetting.tableColDef.filter((x) => x.parentHeaderName).length >
      0
    );
  }

  numberFormatColor(value,displayType) {
    if(value===undefined||value===null) return '';
    const numberFormatColorFraction = displayType == 'number-format-color-fraction' || displayType == 'number-format-color-bg-fraction';
    const fractionDigit = numberFormatColorFraction ? 2 : 0;
    const formatValue = Number(value).toLocaleString('en-US', 
                            { style: 'decimal', minimumIntegerDigits: 1, 
                                minimumFractionDigits: fractionDigit, maximumFractionDigits: fractionDigit });
    return formatValue;
  }

  conditionalRowStyles(dataObj) {
    let styles: any = {};
    if (this.pTableSetting.enabledConditionalRowStyles) {
      const columnHeaders = this.pTableSetting.conditionalRowStyles.filter(x => 
                      this.pTableSetting.tableColDef.find(y => x.columnName == y.internalName));
      columnHeaders.forEach(ch => {
        if (ch.columnValues.indexOf(dataObj[ch.columnName]) > -1) {
          if (ch.rowStyles) { 
            styles = ch.rowStyles; 
          } else {
            styles = {
              'font-weight': 'bold',
              'background-color': '#e8edff',
            }
          }
          return;
        }
      });
    } 
    return styles;
  }
}
