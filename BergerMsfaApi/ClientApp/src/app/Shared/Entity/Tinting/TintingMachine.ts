import { Status } from '../../Enums/status';
import { QueryObject } from '../Common/query-object';

export class TintingMachine {
    id: number;
    territory: string;
    userInfoId: number;
    userFullName: string;
    companyId: number;
    companyName: string;
    noOfActiveMachine: number;
    noOfInactiveMachine: number;
    no: number;
    contribution: number;
    noOfCorrection: number;
    
    constructor(init?: Partial<TintingMachine>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class TintingMachineQuery extends QueryObject {
    territory: string;
    userFullName: string;
    companyName: string;
    
    constructor(init?: Partial<TintingMachineQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.territory = '';
        this.userFullName = '';
        this.companyName = '';
    }
}