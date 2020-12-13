import { Status } from '../../Enums/status';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';


export class SchemeMaster {
    id: number;
    schemeName: string;
    condition: string;
    constructor() {
        this.id = 0;
    }
}

export class SchemeDetail {
    id: number;
    code: string;
    slab: string;
    item: string;
    condition: string;
    date: string;
    vDate: NgbDate
    benefit: string;
    targetVolume: string;
    schemeMasterId: number;
    status:Status

    constructor() {
        this.id = 0;
        this.status = Status.Pending
    }
}