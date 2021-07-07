import { QueryObject } from '../Common/query-object';

export class Brand {
    id: number;
    materialCode: string;
    materialDescription: string;
    materialType: string;
    materialGroupOrBrand: string;
    packSize: string;
    division: string;
    createdDate: string;
    updatedDate: string;
    isCBInstalled: boolean;
    isMTS: boolean;
    isPremium: boolean;
    isEnamel: boolean;
    isLiquid: boolean;
    isPowder: boolean;

    isCBInstalledText: string;
    isMTSText: string;
    isPremiumText: string;
    isEnamelText: string;
    isLiquidText: string;
    isPowderText: string;

    isCBInstalledBtnClass: string;
    isMTSBtnClass: string;
    isPremiumBtnClass: string;
    isEnamelBtnClass: string;
    isLiquidBtnClass: string;
    isPowderBtnClass: string;

    isCBInstalledBtnIcon: string;
    isMTSBtnIcon: string;
    isPremiumBtnIcon: string;
    isEnamelBtnIcon: string;
    isLiquidBtnIcon: string;
    isPowderBtnIcon: string;

    // log details button
    viewDetailsText: string;
    viewDetailsBtnclass: string;
    constructor(init?: Partial<Brand>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class BrandStatus {
    materialOrBrandCode: string;
    propertyName: string;

    constructor(init?: Partial<BrandStatus>) {
        Object.assign(this, init);
    }

    clear() {
        this.materialOrBrandCode = '';
        this.propertyName = '';
    }
}

export class BrandQuery extends QueryObject {
    matrialCode: string;
    matarialDescription: string;
    matarialGroupOrBrand: string;
    matrialCodes: string[];
    brands: string[];

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

