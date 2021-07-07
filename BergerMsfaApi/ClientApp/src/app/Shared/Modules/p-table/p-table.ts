export interface IPTableSetting {
  tableID?: string | 'P-table-001';
  tableClass?: string | 'table table-border';
  tableName?: string | 'p-table-name';
  enabledSerialNo?: boolean | false;
  tableRowIDInternalName?: string | 'Id';
  tableColDef?: colDef[];
  enabledSearch?: boolean | true;
  enabledCheckbox?: boolean | false;
  enabledEditDeleteBtn?: boolean | false;
  enabledEditBtn?: boolean | false;
  enabledDeleteBtn?: boolean | false;
  enabledRecordCreateBtn?: boolean | false;
  enabledRadioBtn?: boolean | false;
  enabledDataLength?: boolean | false;
  enabledPagination?: boolean | true;
  enabledCellClick?: boolean | false;
  enabledColumnResize?: boolean | false;
  enabledStaySelectedPage?: boolean | false;
  enabledColumnFilter?: boolean | false;
  disabledTableReset?: boolean | false;
  pageSize?: number | 10;
  displayPaggingSize?: number | 10;
  checkboxColumnHeader?: boolean | string | 'Select';
  radioBtnColumnHeader?: string | 'Select';
  checkboxCallbackFn?: boolean | null;
  columnNameSetAsClass?: boolean | null;
  enabledColumnSetting?: boolean | false;
  enabledReordering?: boolean | false;
  tableHeaderVisibility?: boolean | true;
  tableFooterVisibility?: boolean | true;
  pTableStyle?: ptableStyle;
  enabledCustomReflow?: boolean | false;
  enabledReflow?: boolean | false;
  enabledAutoScrolled?: boolean | false;
  enabledPdfDownload?: boolean | false;
  enabledExcelDownload?: boolean | false;
  enabledPrint?: boolean | false;
  enabledTotal?: boolean | false;
  totalTitle?: string | 'Total';
  enabledServerSitePaggination?: boolean | false;
  newRecordButtonText?: string | 'New Record';
  newRecordButtonIcon?: string | 'fa fa-plus';
  downloadDataApiUrl?: string | null;
  enabledDetailsEditDeleteBtn?: boolean | false;
  enabledDetailsBtn?: boolean | false;
}

export interface colDef {
  headerName?: string | '';
  width?: string | '';
  internalName?: string;
  className?: string;
  sort?: Boolean | false;
  type?: string;
  displayType?: string;
  onClick?: string | '';
  applyColFilter?: string | 'Apply';
  visible?: boolean | true;
  alwaysVisible?: boolean | false;
  btnTitle?: string | '';
  showTotal?: boolean | false;
  innerBtnIcon?: string | '';
  parentHeaderName?: string;
  colspan?: number;
}

export interface ptableStyle {
  tableOverflow?: boolean | false;
  tableOverflowX?: boolean | false;
  tableOverflowY?: boolean | false;
  overflowContentWidth?: string | '';
  overflowContentHeight?: string | null;
}

export interface IPTableServerQueryObj {
  searchVal: string;
  pageNo: number;
  pageSize: number;
  orderBy: string;
  isOrderAsc: boolean;
}
