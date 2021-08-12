import { Status } from '../../Enums/status';
import { QueryObject } from '../Common/query-object';

export class UniverseReachAnalysis {
    id: number;
    businessArea: string;
    territory: string;
    fiscalYear: string;
    outletNumber: number;
    directCovered: number;
    indirectCovered: number;
    directTarget: number;
    indirectTarget: number;
    indirectManual: number;

    constructor(init?: Partial<UniverseReachAnalysis>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class SaveUniverseReachAnalysis {
    id: number;
    businessArea: string;
    territory: string;
    outletNumber: number;
    directCovered: number;
    indirectCovered: number;
    directTarget: number;
    indirectTarget: number;
    indirectManual: number;
    
    constructor(init?: Partial<SaveUniverseReachAnalysis>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class UniverseReachAnalysisQuery extends QueryObject {
    businessArea: string;
    territories: string[];

    constructor(init?: Partial<UniverseReachAnalysisQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}
