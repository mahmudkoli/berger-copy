import { EnumDealerStatusExcelImportType } from "../../Enums/dealer-info";

export interface DealerStatusExcelExportModel {
    file: any;
    fileName: string;
}

export interface DealerStatusExcelImportModel {
    file: File;
    type: EnumDealerStatusExcelImportType;
}