﻿import { Status } from '../../Enums/status';
import { QueryObject } from '../Common/query-object';

export class CollectionConfig {
    id: number;
    changeableMaxDateDay: number;
    changeableMaxDate: Date;
    changeableMaxDateText: string;
    
    constructor(init?: Partial<CollectionConfig>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class SaveCollectionConfig {
    id: number;
    changeableMaxDateDay: number;
    
    constructor(init?: Partial<CollectionConfig>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class CollectionPlan {
    id: number;
    userId: number;
    userFullName: string;
    businessArea: string;
    territory: string;
    year: number;
    month: number;
    yearMonthText: string;
    slippageAmount: number;
    collectionTargetAmount: number;
    collectionActualAmount: number;
    slippageCollectionActualAmount: number;
    
    changeableMaxDateDay: number;
    changeableMaxDate: Date;
    changeableMaxDateText: string;

    constructor(init?: Partial<CollectionPlan>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class SaveCollectionPlan {
    id: number;
    businessArea: string;
    territory: string;
    slippageAmount: number;
    collectionTargetAmount: number;
    
    constructor(init?: Partial<CollectionPlan>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class CollectionPlanQuery extends QueryObject {
    
    constructor(init?: Partial<CollectionPlanQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}