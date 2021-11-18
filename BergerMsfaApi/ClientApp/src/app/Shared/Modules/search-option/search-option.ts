﻿import { QueryObject } from "src/app/Shared/Entity/Common/query-object";

export class SearchOptionQuery extends QueryObject {
    depot: string;
    dealerType:number;
    salesGroups: string[];
    territories: string[];
    zones: string[];
    fromDate: Date;
    toDate: Date;
    date: Date;
    userId: number;
    dealerId: number;
    creditControlArea: string;
    division: string;
    paintingStageId: number;
    projectStatusId: number;
    painterId: number;
    painterTypeId: number;
    paymentMethodId: number;
    paymentFromId: number;
    materialCodes: string[];
    brands: string[];
    fromMonth: number;
    toMonth: number;
    fromYear: number;
    toYear: number;
    month: number;
    year: number;
    fiscalYear: number
    text1: string;
    text2: string;
    text3: string;
    activitySummary: string;
    valueVolumeResultType: number;
    customerClassificationType: number;
    customerNo: string; // assign when dealer selected

    constructor(init?: Partial<SearchOptionQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SearchOptionSettings {
    searchOptionDef: SearchOptionDef[];
    hasMonthDifference: boolean;
    monthDifferenceCount: number;
    isDealerShow: boolean;
    isSubDealerShow: boolean;
    isDealerSubDealerOptionShow: boolean;

    constructor(init?: Partial<SearchOptionSettings>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SearchOptionDef {
    searchOption: EnumSearchOption;
    isRequiredBasedOnEmployeeRole: boolean;
    isRequired: boolean;
    textLabel: string;
    isAutoGenerated: boolean;
    isReadonly: boolean;

    constructor(init?: Partial<SearchOptionDef>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export enum EnumSearchOption {
    Depot='depot',
    SalesGroup='salesGroups',
    Territory='territories',
    Zone='zones',
    FromDate='fromDate',
    ToDate='toDate',
    Date='date',
    UserId='userId',
    DealerId='dealerId',
    DealerType='dealerType',
    CreditControlArea='creditControlArea',
    Division='division',
    PaintingStageId='paintingStageId',
    ProjectStatusId='projectStatusId',
    PainterId='painterId',
    PainterTypeId='painterTypeId',
    PaymentMethodId='paymentMethodId',
    PaymentFromId='paymentFromId',
    MaterialCode='materialCodes',
    ActivitySummary='activitySummary',
    Brand='brands',
    FromMonth='fromMonth',
    ToMonth='toMonth',
    FromYear='fromYear',
    ToYear='toYear',
    Month='month',
    Year='year',
    FiscalYear='fiscalYear',
    Text1='text1',
    Text2='text2',
    Text3='text3',
    ValueVolumeResultType='valueVolumeResultType',
    CustomerClassificationType='customerClassificationType',
}


export class DealerFilter{
    depots: string;
    salesGroups: string[];
    territories: string[];
    zones: string[];
    salesOffices: string[];
    dealerCategory:number
}

export class BrandFilter {
    brands: string[];
}
