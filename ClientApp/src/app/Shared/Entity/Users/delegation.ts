import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

export class Delegation {
    public id: number;
    public deligatedFromUserId: number;

    public deligatedToUserId: number;

    public fromName: string;
    public toName: string;

    public frombDate: NgbDate;
    public fromDate: string;

    public tobDate: NgbDate;
    public toDate: string;

    public name: string;

    public deligatedFromUserName: string;

    public deligatedToUserName: string;


    constructor() {
        this.id = 0;
    }
}
