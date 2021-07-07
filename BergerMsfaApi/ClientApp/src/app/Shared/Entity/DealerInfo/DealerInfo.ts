import { EnumClubSupreme } from "../../Enums/dealer-info";
import { QueryObject } from "../Common/query-object";

export class DealerInfo {
    id: number;
    customerNo: string;
    customerName: string;
    depot: string;
    salesOffice: string;
    salesGroup: string;
    territory: string;
    zone: string;
    address: string;
    contactNo: string;
    accountGroup: string;
    isExclusive: boolean;
    isLastYearAppointed: boolean;
    isAP: boolean;
    clubSupremeType: EnumClubSupreme;

    isExclusiveText: string;
    isLastYearAppointedText: string;
    isAPText: string;

    isExclusiveBtnClass: string;
    isLastYearAppointedBtnClass: string;
    isAPBtnClass: string;

    isExclusiveBtnIcon: string;
    isLastYearAppointedBtnIcon: string;
    isAPBtnIcon: string;

    clubSupremeTypeDropdown: any;
    clubSupremeTypeDropdownClass: string;

    // log details button
    // viewDetailsText: string;
    // viewDetailsBtnClass: string;

    constructor(init?: Partial<DealerInfo>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerInfoStatus {
    dealerId: number;
    propertyName: string;
    propertyValue: any;

    constructor(init?: Partial<DealerInfoStatus>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerInfoQuery extends QueryObject {
    depot: string;
    salesGroups: string[];
    territories: string[];
    zones: string[];

    constructor(init?: Partial<DealerInfoQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.depot = '';
        this.salesGroups = [];
        this.territories = [];
        this.zones = [];
    }
}