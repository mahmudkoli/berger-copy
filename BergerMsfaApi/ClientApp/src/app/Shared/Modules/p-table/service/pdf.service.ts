import { Injectable } from "@angular/core";
import { IPTableSetting } from "../p-table.component";
declare var jsPDF: any;
@Injectable()
export class PDFService {
    constructor() {

    }
    public pTableSetting: IPTableSetting;
    public pTableData: any[] = [];

    exportPDF(pTableSetting: IPTableSetting, pTableData: any[]) {
        this.pTableSetting = pTableSetting;
        this.pTableData = pTableData || [];

        let title = (this.pTableSetting.tableName.toString());
        var columns = [];
        this.pTableSetting.tableColDef.forEach((col: any) => {
            columns.push({ title: col.headerName, dataKey: col.internalName });
        });
        var rows = this.pTableData || [];


        // Only pt supported (not mm or in) 
        var doc = new jsPDF('p', 'pt');
        var header = function (data) {
            doc.setFontSize(18);
            doc.setTextColor(40);
            //doc.setFontStyle('normal');
            //doc.addImage(headerImgData, 'JPEG', data.settings.margin.left, 20, 50, 50);
            doc.text("Testing Report", data.settings.margin.left, 50);
        };

        var options = {
            beforePageContent: header,
            margin: {
                top: 200
            },
            addPageContent: function (data) {
                doc.text(title, 40, 30);
            },
            startY: doc.autoTableEndPosY() + 40
        };
        doc.autoTable(columns, rows, options);
        doc.save(this.pTableSetting.tableName + '.pdf');
    }
}