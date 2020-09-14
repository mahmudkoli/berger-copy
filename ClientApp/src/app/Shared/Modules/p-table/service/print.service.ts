import { Injectable } from "@angular/core";
import { IPTableSetting } from "../p-table.component";

@Injectable()
export class PrintService {
    constructor() { }
    public pTableData: any[] = [];
    printContent(pTableSetting: IPTableSetting, pTableData: any[] ) {
        this.pTableData = pTableData
        let tableHeader = "", tableBody = "";
        let slNo = 0;
        //add SL 
        if (pTableSetting.enabledSerialNo) {
            tableHeader = tableHeader + ` <td  align="center">ক্রমিক </td> `;
        }
        pTableSetting.tableColDef.forEach((col: any) => {
            tableHeader = tableHeader + ` <td  align="center">${col.headerName} </td> `;
        });

        //bind table body
        pTableData.forEach((rec: any) => {
            tableBody = tableBody + ` <tr style="page-break-inside: avoid;"> `;
            if (pTableSetting.enabledSerialNo) {
                slNo = slNo + 1;
                tableBody = tableBody + ` <td  align="center">${slNo} </td> `;
            }
            pTableSetting.tableColDef.forEach((col: any) => {
                if(col.displayType=='number'){
                    tableBody = tableBody + ` <td  align="center">${(rec[col.internalName] || 0).toLocaleString()} </td> `;
                }else{
                    tableBody = tableBody + ` <td  align="center">${rec[col.internalName] || ''} </td> `;
                }
              //  tableBody = tableBody + ` <td  align="center">${rec[col.internalName] || ''} </td> `;
            });
            tableBody = tableBody + ` </tr> `;
        });

        //to show total 
        if (pTableSetting.enabledTotal) {
            tableBody = tableBody + ` <tr style="page-break-inside: avoid;"> `;
            pTableSetting.tableColDef.forEach((col: any, index: number) => {
                if (index == 0) {
                    tableBody = tableBody + ` <td  align="center"><b>${pTableSetting.totalTitle || 'Total'} </b></td> `;
                } else {
                    if (col.showTotal) {
                        tableBody = tableBody + ` <td  align="center"><b>${(this.fnSummationTotal(col.internalName)||0).toLocaleString()} </b></td> `;
                    } else {
                        tableBody = tableBody + ` <td  align="center"></td> `;
                    }
                }

            });

            tableBody = tableBody + ` </tr> `;
        }

        let printContents, popupWin;
        printContents = `<table style="width:100%; border:0">
    <tr>
    <td colspan="2" align="center"><b>Compnay Title</b></td> </tr>
    <tr> <td colspan="2" align="center"><b</b>Company Name </td> </tr>
    <tr> <td colspan="2" align="center">Company Address</td> </tr>
    <tr> <td colspan="2" align="center"><b>${pTableSetting.tableName}</b></td> </tr>
         <tr>
          <td  colspan="2">         
               <table border="1" style="width:100%; border-collapse: collapse; font-size:10pt;">
                 <tbody>
                   <tr style="background-color: rgba(0, 0, 0, 0.05);font-weight:bold;">
                   ${   tableHeader}
                   </tr>
                 </tbody>
                 <tr>
                 ${ tableBody}
                 </tr>
               </table>
           </td>
          </tr>
      </table>
      `;
        popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
        popupWin.document.open();
        popupWin.document.write(`
        <html>
          <head>
            <title>${pTableSetting.tableName} (system generated:B-Track)</title>
            <style>
            //........Customized style.......
            </style>
          </head>
      <body onload="window.print();window.close()">${printContents}</body>
        </html>`);
        popupWin.document.close();
    }

    fnSummationTotal(columnName: string) {
        let sum: number = 0;
        this.pTableData.forEach((rec: any) => {
            sum = Number(sum) + Number(rec[columnName] == null ? 0 : isNaN(rec[columnName]) == true ? 0 : rec[columnName] || 0);
        });
        return sum;
    }
}