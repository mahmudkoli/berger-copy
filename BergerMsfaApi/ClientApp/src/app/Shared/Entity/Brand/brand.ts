import { QueryObject } from '../Common/query-object';
import { Dropdown } from '../Setup/dropdown';

export class Brand {
    id: number;
    matrialCode: string;
    matarialDescription: string;
    mtart: string;
    matarialGroupOrBrand: string;
    packSize: string;
    division: string;
    ersda: string;
    laeda: string;
    isCBInstalled: boolean;
    isMTS: boolean;
    isPremium: boolean;
    isCBInstalledText: string;
    isMTSText: string;
    isPremiumText: string;
    
    constructor(init?: Partial<Brand>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class BrandStatus {
    materialCode: string;
    propertyName: string;
    
    constructor(init?: Partial<BrandStatus>) {
        Object.assign(this, init);
    }

    clear() {
        this.materialCode = '';
        this.propertyName = '';
    }
}

export class BrandQuery extends QueryObject {
    matrialCode: string;
    matarialDescription: string;
    matarialGroupOrBrand: string;
    
    constructor(init?: Partial<BrandQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.matrialCode = '';
        this.matarialDescription = '';
        this.matarialGroupOrBrand = '';
    }
}