import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';
import { colDef, IPTableSetting } from '../p-table';

const EXCEL_TYPE =
  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';

@Injectable()
export class ExcelService {
  constructor() {}
  public pTableSetting: IPTableSetting;
  public exportAsExcelFile(
    pTableData: any[],
    pTableSetting: IPTableSetting
  ): void {
    let excelFileName = pTableSetting.tableName;
    let json: any[] = [];
    let colDef: colDef[] = pTableSetting.tableColDef;
    pTableData.forEach((rec: any) => {
      let column = new Object();
      colDef.forEach((col: colDef) => {
        column[col.headerName] = rec[col.internalName];
      });
      json.push(column);
    });

    if (pTableSetting.enabledTotal) {
      let column = new Object();
      colDef.forEach((col: colDef, index: number) => {
        if (index == 0) {
          column[col.headerName] = 'Total';
        } else if (col.showTotal) {
          column[col.headerName] = this.fnSummationTotal(
            col.internalName,
            pTableData
          );
        } else {
          column[col.headerName] = '';
        }
      });
      json.push(column);
    }

    //const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);
    // const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
    //   [
    //     { A: 'Company Name' },
    //     { A: 'Company Address' },
    //     { A: pTableSetting.tableName },
    //     { A: '' },
    //   ],
    //   { header: ['A'], skipHeader: true }
    // );

    let worksheet: XLSX.WorkSheet = XLSX.utils.book_new();
    const merge = [];
    if (
      pTableSetting.tableColDef.filter((x) => x.parentHeaderName).length > 0
    ) {
      const newHeaderJsonArray = [];
      let column = 0;
      pTableSetting.tableColDef.forEach((element) => {
        //let sheetCell = (column + 1 + 9).toString(36).toUpperCase();
        if (!element.parentHeaderName) {
          newHeaderJsonArray.push(element.headerName);

          merge.push({
            s: { r: 0, c: column },
            e: { r: 1, c: column },
          });
        } else if (
          newHeaderJsonArray.filter((x) => x == element.parentHeaderName)
            .length == 0
        ) {
          newHeaderJsonArray.push(element.parentHeaderName);
          const length = pTableSetting.tableColDef.filter(
            (x) => x.parentHeaderName == element.parentHeaderName
          ).length;
          merge.push({
            s: { r: 0, c: column },
            e: { r: 0, c: column + length - 1 },
          });
        } else {
          newHeaderJsonArray.push('');
        }
        column++;
      });
      XLSX.utils.sheet_add_aoa(worksheet, [newHeaderJsonArray]);
    }
    // worksheet['!merges'] = [
    //   { s: { r: 0, c: 0 }, e: { r: 0, c: colDef.length - 1 } } /* A1:A2 */,
    //   { s: { r: 1, c: 0 }, e: { r: 1, c: colDef.length - 1 } },
    //   { s: { r: 2, c: 0 }, e: { r: 2, c: colDef.length - 1 } },
    // ];

    worksheet['!merges'] = merge;
    XLSX.utils.sheet_add_json(worksheet, json, {
      skipHeader: false,
      origin: 'A2',
    });
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ['data'],
      Props: {
        Title: 'BRTC',
        Author: 'Palash',
      },
    };
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });
    //const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'buffer' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: EXCEL_TYPE,
    });
    //FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
    FileSaver.saveAs(data, fileName + EXCEL_EXTENSION);
  }

  fnSummationTotal(columnName: string, pTableData: any[]) {
    let sum: number = 0;
    pTableData.forEach((rec: any) => {
      sum =
        Number(sum) +
        Number(
          rec[columnName] == null
            ? 0
            : isNaN(rec[columnName]) == true
            ? 0
            : rec[columnName] || 0
        );
    });
    return sum;
  }
}
