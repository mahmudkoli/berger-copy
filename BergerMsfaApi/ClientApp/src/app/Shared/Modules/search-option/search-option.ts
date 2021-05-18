import { QueryObject } from "src/app/Shared/Entity/Common/query-object";

export class SearchOptionQuery extends QueryObject {
    depot: string;
    salesGroups: string[];
    territories: string[];
    zones: string[];
    fromDate: Date;
    toDate: Date;
    userId: number;
    dealerId: number;
    creditControlArea: string;
    paintingStageId: number;
    projectStatusId: number;
    painterId: number;
    painterTypeId: number;
    paymentMethodId: number;
    materialCodes:string[];
    brands:string[];
    fromMonth: number;
    toMonth: number;
    fromYear: number;
    toYear: number;
    month: number;
    year: number;
    text1: string;
    text2: string;
    text3: string;

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
    UserId='userId',
    DealerId='dealerId',
    CreditControlArea='creditControlArea',
    PaintingStageId='paintingStageId',
    ProjectStatusId='projectStatusId',
    PainterId='painterId',
    PainterTypeId='painterTypeId',
    PaymentMethodId='paymentMethodId',
    MaterialCode='materialCodes',
    Brand='brands',
    FromMonth='fromMonth',
    ToMonth='toMonth',
    FromYear='fromYear',
    ToYear='toYear',
    Month='month',
    Year='year',
    Text1='text1',
    Text2='text2',
    Text3='text3',
}
