import { QueryObject } from '../Common/query-object';
import { Dropdown } from '../Setup/dropdown';

export class Lead {
    id: number;
    
    constructor(init?: Partial<Lead>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class LeadQuery extends QueryObject {
    title: string;
    
    constructor(init?: Partial<LeadQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}