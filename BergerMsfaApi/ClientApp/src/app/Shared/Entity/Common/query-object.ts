
export class QueryObject {
    sortBy: string;
    isSortAscending: boolean;
    page: number;
    pageSize: number;
    globalSearchValue: string;
    
    constructor(init?: Partial<QueryObject>) {
        Object.assign(this, init);
    }

    clear() {
    }
}
