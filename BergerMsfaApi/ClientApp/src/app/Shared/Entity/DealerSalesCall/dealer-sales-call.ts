import { QueryObject } from '../Common/query-object';
import { Dropdown } from '../Setup/dropdown';

export class DealerSalesCall {
    id: number;
    
    constructor(init?: Partial<DealerSalesCall>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class DealerSalesCallQuery extends QueryObject {
    title: string;
    
    constructor(init?: Partial<DealerSalesCallQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}