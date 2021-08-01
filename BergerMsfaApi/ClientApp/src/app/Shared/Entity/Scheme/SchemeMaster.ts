import { QueryObject } from '../Common/query-object';

export class SchemeMaster {
    id: number;
    schemeName: string;
    condition: string;
    businessArea: string;

    constructor(init?: Partial<SchemeMaster>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.schemeName = '';
        this.condition = '';
    }
}

export class SaveSchemeMaster {
    id: number;
    schemeName: string;
    condition: string;
    businessArea: string;

    constructor(init?: Partial<SchemeMaster>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.schemeName = '';
        this.condition = '';
    }
}

export class SchemeMasterQuery extends QueryObject {
    schemeName: string;

    constructor(init?: Partial<SchemeMasterQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.schemeName = '';
    }
}

export class SchemeDetail {
    id: number;
    //National Scheme (Brand)
    code: string;
    brand: string;
    rateInLtrOrKg: string;
    rateInSKU: string;

    //National Scheme (Value)
    slab: string;
    condition: string;
    benefitStartDate: Date;
    benefitEndDate?: Date;
    benefitStartDateText: string;
    benefitEndDateText: string;


    schemeMasterId: number;
    schemeMasterName: string;
    schemeMasterCondition: string;
    status: number;

    statusText: string;

    constructor(init?: Partial<SchemeDetail>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.schemeMasterId = null;
    }
}

export class SaveSchemeDetail {
    id: number;
    //National Scheme (Brand)
    code: string;
    brand: string;
    rateInLtrOrKg: string;
    rateInSKU: string;

    //National Scheme (Value)
    slab: string;
    condition: string;
    benefitStartDate: Date;
    benefitEndDate?: Date;


    schemeMasterId: number;
    status: number;

    constructor(init?: Partial<SchemeDetail>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.schemeMasterId = null;
    }
}

export class SchemeDetailQuery extends QueryObject {
    schemeMasterName: string;

    constructor(init?: Partial<SchemeDetailQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.schemeMasterName = '';
    }
}