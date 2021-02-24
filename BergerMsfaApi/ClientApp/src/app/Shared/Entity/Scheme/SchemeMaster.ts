import { Status } from '../../Enums/status';

export class SchemeMaster {
    id: number;
    schemeName: string;
    condition: string;
    
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
    
    constructor(init?: Partial<SchemeMaster>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.schemeName = '';
        this.condition = '';
    }
}

export class SchemeDetail {
    id: number;
    //National Scheme (Brand)
    code: string;
    brand: string;
    rateInLtrOrKg: string;
    rateInDrum: string;

    //National Scheme (Value)
    slab: string;
    condition: string;
    benefitDate: string;

    //Painter Scheme
    schemeId: string;
    material: string;
    targetVolume: string;

    //Common
    benefit: string;

    schemeMasterId: number;
    schemeMasterName: string;
    schemeMasterCondition: string;
    status: number;
    
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
    rateInDrum: string;

    //National Scheme (Value)
    slab: string;
    condition: string;
    benefitDate: string;

    //Painter Scheme
    schemeId: string;
    material: string;
    targetVolume: string;

    //Common
    benefit: string;

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