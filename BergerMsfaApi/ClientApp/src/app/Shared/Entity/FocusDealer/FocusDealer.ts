import { QueryObject } from "../Common/query-object";

export class FocusDealer {
    id: number;
    dealerId: number;
    employeeId: string;
    validFrom: Date;
    validTo: Date;
    validFromText: string;
    validToText: string;
    dealerName: string;
    userFullName: string;
    depot: string;
    territory: string;
    zone: string;

    constructor(init?: Partial<FocusDealer>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SaveFocusDealer {
    id: number;
    dealerId: number;
    employeeId: string;
    validFrom: Date;
    validTo: Date;

    constructor(init?: Partial<SaveFocusDealer>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class FocusDealerQuery extends QueryObject {
    depot: string;
    territories: string[];
    zones: string[];

    constructor(init?: Partial<FocusDealerQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.depot = '';
        this.territories = [];
        this.zones = [];
    }
}